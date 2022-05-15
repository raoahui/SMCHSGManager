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
                                                             where (r.MemberInfo.Name.Contains(searchContent) || r.PayMethod.Name.Contains(searchContent) || searchContent == null || r.MemberInfo.MemberNo == memberNO) 
                                                             select new MemberFeePaymentListViewModel
                                                             {
                                                                 Name = r.MemberInfo.Name,
                                                                 MemberNo = r.MemberInfo.MemberNo.Value,
                                                                 IMemberID = r.IMemberID,
                                                                 FromDate = r.FromDate,
                                                                 ToDate = r.ToDate,
                                                                 PayAmount = r.PayAmount,
                                                                 PaymentMethod = r.PayMethod.Name,
                                                                 ReceivedDate = r.ReceivedDate,
                                                             }).OrderByDescending(a=>a.ReceivedDate).ToList();

            if (memberID.HasValue)
            {
                viewModel = viewModel.Where(a => a.IMemberID == memberID.Value).OrderByDescending(a => a.ReceivedDate).ToList();
            }
			ViewData["TotalPages"] = (int)Math.Ceiling((float)viewModel.Count() / _pageSize);

			if ((int)ViewData["TotalPages"] < currentPage)
			{
				currentPage = 1;
			}
			ViewData["CurrentPage"] = currentPage;

            viewModel = (viewModel.AsQueryable().OrderBy(a => a.ReceivedDate).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

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

        public List<MemberFeeExpiredDateInfo> updateMemberFeeExipredDate()
		{

            List<MemberFeeExpiredDateInfo> latestMemberFeePayments = (from r in _entities.MemberFeePayments
                                                                      where r.MemberInfo.IsActive && r.MemberInfo.Name != "DP"
                                                                      orderby r.ToDate descending
                                                                      group r by new
                                                                      {
                                                                          ID = r.MemberInfo.MemberID,
                                                                          No = r.MemberInfo.MemberNo,
                                                                          Name = r.MemberInfo.Name
                                                                      } into h
                                                                      select new MemberFeeExpiredDateInfo()
                                                                      {
                                                                          MemberID = h.Key.ID,
                                                                          MemberNo = h.Key.No.Value,
                                                                          Name = h.Key.Name,
                                                                          MemberFeeExpiredDate = h.Max(a => a.ToDate),
                                                                      }).OrderBy(a=>a.MemberNo).ToList();
            
            return latestMemberFeePayments;
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

            viewModel.MemberFeePayment.FromDate = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).Date;
			viewModel.MemberFeePayment.ToDate = (new DateTime(DateTime.Today.Year, 12, 31)).Date;
			viewModel.MemberFeePayment.PayAmount = 12 * 20;
			viewModel.MemberFeePayment.PayMethodID = 1;
			return View(viewModel);
        }

        private MemberFeePaymentViewModel GetMemberFeePaymentViewModel()
        {
            var viewModel = new MemberFeePaymentViewModel
            {
                MemberFeePayment = new MemberFeePayment(),
                MemberInfos = BlackListMemberController.GetMemberNameSelectList(Guid.Empty),
                PayMethod = _entities.PayMethods.Where(a=>a.ID != 4).ToList() // exclude Giro
            };
            return viewModel;
        }

        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        //
		// POST: /MemberFeePayment/Create

        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
        public ActionResult Create(FormCollection collection, MemberFeePayment memberFeePayment)
        {
			memberFeePayment.IMemberID = Guid.Parse(collection.Get("MemberFeePayment.IMemberID"));
			memberFeePayment.ReceivedDate = DateTime.Now.ToUniversalTime().AddHours(8);

            try
            {
                CheckTheItemsValid(memberFeePayment);
				_entities.AddToMemberFeePayments(memberFeePayment);
                _entities.SaveChanges();

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

        private void CheckTheItemsValid(MemberFeePayment memberFeePayment)
        {
            if (memberFeePayment.ToDate < memberFeePayment.FromDate)
            {
                ModelState.AddModelError("memberFeePayment.FromDate", "FromDate should be earlier than ToDate!");
                throw new Exception();
            }
            else if (_entities.MemberFeePayments.Any(a => a.IMemberID == memberFeePayment.IMemberID &&
                                                 (a.ToDate >= memberFeePayment.FromDate && a.FromDate <= memberFeePayment.ToDate)))  // don't overlap!
            {
                ModelState.AddModelError("memberFeePayment.FromDate", "There is a record in this period already for this initiate!");
                throw new Exception();
            }
            else if (memberFeePayment.PayMethodID != 4)
            {
                DateTime nextMonthDate = memberFeePayment.ToDate.AddDays(1);
                if (memberFeePayment.FromDate.Day != 1)
                {
                    ModelState.AddModelError("memberFeePayment.FromDate", "FromDate should be 1st day of month!");
                    throw new Exception();
                }
                else if (nextMonthDate.Day != 1)
                {
                    ModelState.AddModelError("memberFeePayment.ToDate", "ToDate should be last day of month!");
                    throw new Exception();
                }
                else
                {
                    int month = MonthDifference(nextMonthDate, memberFeePayment.FromDate);
                    decimal mountPerMonth = memberFeePayment.PayAmount / month;
                    if (mountPerMonth != 2 && mountPerMonth != 10 && mountPerMonth != 20)
                    {
                        ModelState.AddModelError("memberFeePayment.PayAmount", "Pay amount is not match the period FromDate ~ ToDate!");
                        throw new Exception();
                    }
                }
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateGiro()
        {
            var viewModel = new MemberFeePaymentViewModel
            {
                MemberFeePayment = new MemberFeePayment(),
                MemberInfos = BlackListMemberController.GetMemberNameSelectList(Guid.Empty),
            };

            viewModel.MemberFeePayment.FromDate = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).Date;
            viewModel.MemberFeePayment.PayAmount = 20;
            viewModel.MemberFeePayment.PayMethodID = 4;
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
        public ActionResult CreateGiro(FormCollection collection, MemberFeePayment memberFeePayment)
        {
            memberFeePayment.IMemberID = Guid.Parse(collection.Get("MemberFeePayment.IMemberID"));
            memberFeePayment.ReceivedDate = DateTime.Now.ToUniversalTime().AddHours(8);
            memberFeePayment.ToDate = MemberFeePayment.ToDateGiro;
            memberFeePayment.PayMethodID = 4;

            try
            {
                CheckTheItemsValid(memberFeePayment);
                _entities.AddToMemberFeePayments(memberFeePayment);
                _entities.SaveChanges();

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

                return View(viewModel);
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
            DateTime toDate = DateTime.Parse(collection.GetValues("MemberFeePayment.ToDate")[0]);

            try
			{
                // cancel Giro need to change the ToDate, but as this is key, so need to add a new item first, then delete the old one.
                if (memberFeePayment.PayMethodID == 4 && toDate != MemberFeePayment.ToDateGiro)
                {
                    MemberFeePayment mep = _entities.MemberFeePayments.Single(a => a.IMemberID == IMemberID && a.FromDate == FromDate && a.ToDate == ToDate);
                    _entities.DeleteObject(mep);
                    _entities.SaveChanges();

                    mep.ToDate = toDate;
                    _entities.AddToMemberFeePayments(mep);
                }
                else
                {
                    UpdateModel(memberFeePayment, "MemberFeePayment");
                    if (memberFeePayment.PayMethodID != 4)
                    {
                        DateTime nextMonthDate = memberFeePayment.ToDate.AddDays(1);
                        int month = MonthDifference(nextMonthDate, memberFeePayment.FromDate);
                        decimal mountPerMonthe = memberFeePayment.PayAmount / month;
                        if (mountPerMonthe != 2 && mountPerMonthe != 10 && mountPerMonthe != 20)
                        {
                            ModelState.AddModelError("memberFeePayment.PayAmount", "Pay amount is not match the period FromDate ~ ToDate!.");
                            throw new Exception();
                        }
                    }
                }
                _entities.SaveChanges();
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
                //var viewModel = GetMemberFeePaymentViewModel(IMemberID, FromDate, ToDate);
                  
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
