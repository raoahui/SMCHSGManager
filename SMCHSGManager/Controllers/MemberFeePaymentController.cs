using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;
using System.Globalization;

namespace SMCHSGManager.Controllers
{
    public class MemberFeePaymentController : Controller
    {
		private SMCHDBEntities _entities = new SMCHDBEntities();
		private int _pageSize = 50;
		//
        // GET: /MemberFeePayment/

		[Authorize]
		public ActionResult Index(string sort, int? page, string searchContent, Guid? memberID)
        {
			var currentPage = page ?? 1;
			ViewData["SortItem"] = sort;
			sort = sort ?? "Name";
			ViewData["PageSize"] = _pageSize;

			ViewData["searchContent"] = searchContent;

			int memberNO = 0;
			EventSignatureController erc = new EventSignatureController();
			if (!string.IsNullOrEmpty(searchContent) && erc.IsInteger(searchContent))
			{
				memberNO = int.Parse(searchContent);
			}

            List<MemberFeePaymentListViewModel> viewModel = (from r in _entities.MemberFeePayments
															 where (r.MemberInfo.Name.Contains(searchContent) || searchContent == null || r.MemberInfo.MemberNo == memberNO ) && r.MemberInfo.IsActive
                                                             select new MemberFeePaymentListViewModel
                                                             {
                                                                 Name = r.MemberInfo.Name,
                                                                 MemberNo = r.MemberInfo.MemberNo.Value,
                                                                 IMemberID = r.IMemberID,
                                                                 FromDate = r.FromDate,
                                                                 ToDate = r.ToDate,
                                                                 PayAmount = r.PayAmount,
                                                                 PaymentMethod = r.PayMethod.Name,
                                                                 ReceievedDate = r.ReceivedDate,
                                                             }).OrderByDescending(a=>a.ReceievedDate).ToList();

             if (memberID.HasValue)
            {
                viewModel = viewModel.Where(a => a.IMemberID == memberID.Value).OrderByDescending(a => a.ReceievedDate).ToList();
            }
			ViewData["TotalPages"] = (int)Math.Ceiling((float)viewModel.Count() / _pageSize);

			if ((int)ViewData["TotalPages"] < currentPage)
			{
				currentPage = 1;
			}
			ViewData["CurrentPage"] = currentPage;

            viewModel = (viewModel.AsQueryable().OrderBy(a => a.ReceievedDate).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

			if (viewModel.Count() == 0)
			{
				ViewData["Message"] = "There is no Member Fee Payment record in database, please use \"Create New\" button to create new one.";
			}

			return View(viewModel);
        }

        public ActionResult MemberFeeExpiredDateList()
        {
            List<MemberFeePayment> latestMemberFeePayments = (from r in _entities.MemberFeePayments
                                                              orderby r.ToDate descending
                                                              group r by r.IMemberID into h
                                                              select new MemberFeePayment
                                                              {
                                                                  IMemberID = h.Key,
                                                                  ToDate = h.Max(a => a.ToDate),
                                                              }).ToList();
            return View();
        }
  

        //
        // GET: /MemberFeePayment/Details/5

        //[Authorize]
        //public ActionResult Details(Guid IMemberID, DateTime FromDate, DateTime ToDate)
        //{
        //    MemberFeePayment mep = _entities.MemberFeePayments.Single(a => a.IMemberID == IMemberID && a.FromDate == FromDate && a.ToDate == ToDate);
        //    return View(mep);
        //}

        //
        // GET: /MemberFeePayment/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            var viewModel = GetMemberFeePaymentViewModel();

			viewModel.MemberFeePayment.FromDate = (new DateTime(DateTime.Today.Year, 1, 1)).Date;
			viewModel.MemberFeePayment.ToDate = (new DateTime(DateTime.Today.Year, 12, 31)).Date;
			viewModel.MemberFeePayment.PayAmount = 12 * 20;
			viewModel.MemberFeePayment.PayMethodID = 3;
			return View(viewModel);
        }

        private MemberFeePaymentViewModel GetMemberFeePaymentViewModel()
        {
            var viewModel = new MemberFeePaymentViewModel
            {
                MemberFeePayment = new MemberFeePayment(),
                MemberInfos = BlackListMemberController.GetMemberNameSelectList(Guid.Empty),
                PayMethod = _entities.PayMethods.ToList(),
            };
            return viewModel;
        } 

  		//
		// POST: /MemberFeePayment/Create

        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
         public ActionResult Create(FormCollection collection, MemberFeePayment memberFeePayment)
        {
			memberFeePayment.IMemberID = Guid.Parse(collection.Get("MemberFeePayment.IMemberID"));
			memberFeePayment.ReceivedDate = DateTime.Now.ToUniversalTime().AddHours(8);

			if (memberFeePayment.PayMethodID == 1)
			{
				memberFeePayment.ToDate = new DateTime(2020, 12, 31);
			}

            try
            {
				if (_entities.MemberFeePayments.Any(a => a.IMemberID == memberFeePayment.IMemberID &&
												 (memberFeePayment.FromDate >= a.FromDate && memberFeePayment.ToDate
												 <= a.ToDate)))
				{
					ModelState.AddModelError(string.Empty, "There is a record in this period already for this initiate!"); 
					throw new Exception();
				}

				_entities.AddToMemberFeePayments(memberFeePayment);
                _entities.SaveChanges();

				SaveLatestMemberFeeToDateToMemberInfo(memberFeePayment.IMemberID);
              
                return RedirectToAction("Index");
            }
            catch
            {
				var viewModel = new MemberFeePaymentViewModel
				{
					MemberFeePayment = memberFeePayment,
					MemberInfos = BlackListMemberController.GetMemberNameSelectList(memberFeePayment.IMemberID),
					PayMethod = _entities.PayMethods.ToList(),
				};
			
				//var viewModel = GetMemberFeePaymentViewModel();

                return View(viewModel);
            }
        }

        private void SaveLatestMemberFeeToDateToMemberInfo(Guid memberID)
        {
            DateTime latestToDate = (from r in _entities.MemberFeePayments
                                     where r.IMemberID == memberID
                                     orderby r.ToDate descending
                                     select r.ToDate).FirstOrDefault();

            MemberInfo memberInfo = _entities.MemberInfos.Single(a => a.MemberID == memberID);
            if (memberInfo.MemberFeeExpiredDate < latestToDate)
            {
                memberInfo.MemberFeeExpiredDate = latestToDate;
                _entities.SaveChanges();
            }
        }
        
        //
        // GET: /MemberFeePayment/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(Guid IMemberID, DateTime FromDate, DateTime ToDate)
        {
			var viewModel = GetMemberFeePaymentViewModel(IMemberID, FromDate, ToDate);

			return View(viewModel);
		}

		private MemberFeePaymentViewModel GetMemberFeePaymentViewModel(Guid IMemberID, DateTime FromDate, DateTime ToDate)
		{
			var viewModel = new MemberFeePaymentViewModel
			{
				MemberFeePayment = _entities.MemberFeePayments.Single(a => a.IMemberID == IMemberID && a.FromDate == FromDate && a.ToDate == ToDate),
                MemberInfos = BlackListMemberController.GetMemberNameSelectList(IMemberID),
                PayMethod = _entities.PayMethods.ToList(),
			};

			return viewModel;
		}

        //
        // POST: /MemberFeePayment/Edit/5

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(Guid IMemberID, DateTime FromDate, DateTime ToDate, FormCollection collection)
		{

            MemberFeePayment memberFeePayment = _entities.MemberFeePayments.Single(a => a.IMemberID == IMemberID && a.FromDate == FromDate && a.ToDate == ToDate);
			try
			{
                UpdateModel(memberFeePayment, "MemberFeePayment");
                _entities.SaveChanges();

				SaveLatestMemberFeeToDateToMemberInfo(memberFeePayment.IMemberID);

				return RedirectToAction("Index");
			}
			catch
			{
                var viewModel = GetMemberFeePaymentViewModel(IMemberID, FromDate, ToDate);
                  
                return View(viewModel);
			}
		}



        //
        // GET: /MemberFeePayment/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(Guid IMemberID, DateTime FromDate, DateTime ToDate)
		{
			MemberFeePayment mep = _entities.MemberFeePayments.Single(a => a.IMemberID == IMemberID && a.FromDate == FromDate && a.ToDate == ToDate);
			ViewData["Name"] = _entities.MemberInfos.Single(a => a.MemberID == IMemberID).Name;

			return View(mep);
        }

        //
        // POST: /MemberFeePayment/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(Guid IMemberID, DateTime FromDate, DateTime ToDate, FormCollection collection)
        {
			if (IMemberID != Guid.Empty)
            {
				MemberFeePayment mep = _entities.MemberFeePayments.Single(a => a.IMemberID == IMemberID && a.FromDate == FromDate && a.ToDate == ToDate);
				_entities.DeleteObject(mep);
				_entities.SaveChanges();

                return RedirectToAction("Index");
            }
			return View("Deleted");
        }
    }
}
