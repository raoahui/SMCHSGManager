using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;
//using System.Web.Routing;
//using System.Web.Routing.RequestContext;

namespace SMCHSGManager.Controllers
{

    public class OrdinaryMemberController : Controller
    {

		private SMCHDBEntities _entities = new SMCHDBEntities();

		private int _pageSize = 100;

		//
		// GET: /OrdinaryMember/
		[Authorize(Roles = "Administrator")]
		public ActionResult Index(string sort, int? page, string searchContent, int? IsActive)
		{

			var currentPage = page ?? 1;
			ViewData["SortItem"] = sort;
			sort = sort ?? "UserName";
			ViewData["PageSize"] = _pageSize;

			ViewData["searchContent"] = searchContent;

			int memberNO = 0;
			EventSignatureController erc = new EventSignatureController();
			if (!string.IsNullOrEmpty(searchContent) && erc.IsInteger(searchContent))
			{
				memberNO = int.Parse(searchContent);
			}

			if (IsActive == null)
			{
				IsActive = 1;
			}
			ViewData["IsActive"] = IsActive;

  			List<PublicMemberInfo> memberInfos = (from r in _entities.MemberInfos //OrdinaryMemberInfos
                                                 where (r.Name.Contains(searchContent) || searchContent == null || r.MemberNo == memberNO)
 												 && (r.IsActive && IsActive == 1 || !r.IsActive && IsActive == 2 || IsActive == 3)
												 select new PublicMemberInfo()
												 {
													 ID = r.MemberID,
													 Name = r.Name,
													 DateOfInitiation = r.DateOfInitiation,
													 IDCardNo = r.IDCardNo,
													 ContactNo = r.ContactNo,
													 Gender = r.Gender.Name,
													 InitiateType = r.InitiateType.Name,
													 IsActive = r.IsActive,
													 MemberNo = r.MemberNo,
												 }).ToList();

            MemberFeePaymentController mfpc = new MemberFeePaymentController ();
            List<MemberFeeExpiredDateInfo> latestMemberFeePayments = mfpc.updateMemberFeeExipredDate();
            foreach (PublicMemberInfo pmi in memberInfos)
            {
                MembershipUser user = Membership.GetUser(pmi.ID);
                pmi.Email = user.Email;
                if(latestMemberFeePayments.Any(a=>a.MemberID == pmi.ID))
                {
                    pmi.MemberFeeExpiredDate = latestMemberFeePayments.SingleOrDefault(a => a.MemberID == pmi.ID).MemberFeeExpiredDate;
                }
            }

			ViewData["TotalPages"] = (int)Math.Ceiling((float)memberInfos.Count() / _pageSize);

			if ((int)ViewData["TotalPages"] < currentPage)
			{
				currentPage = 1;
			}
			ViewData["CurrentPage"] = currentPage;

			var sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(a => a.MemberNo).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
			TempData["viewModel"] = memberInfos.AsQueryable().Where(a=>a.MemberNo.HasValue).OrderBy(a => a.MemberNo).ToList();

			if (sort == "Name")
			{
				sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(a => a.Name).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
				TempData["viewModel"] = memberInfos.AsQueryable().Where(a => a.MemberNo.HasValue).OrderBy(a => a.Name).ToList();
			}
			else if (sort == "IDCardNo")
			{
				sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(a => a.IDCardNo).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
				TempData["viewModel"] = memberInfos.AsQueryable().Where(a => a.MemberNo.HasValue).OrderBy(a => a.IDCardNo).ToList();
			}
			else if (sort == "ContactNo")
			{
				sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(a => a.ContactNo).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
				TempData["viewModel"] = memberInfos.AsQueryable().Where(a => a.MemberNo.HasValue).OrderBy(a => a.ContactNo).ToList();
			}
            else if (sort == "MemberFeeExpiredDate")
            {
                sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(a => a.MemberFeeExpiredDate).ThenBy(a => a.MemberNo).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
                TempData["viewModel"] = memberInfos.AsQueryable().Where(a => a.MemberNo.HasValue && a.MemberNo < 999).OrderBy(a => a.MemberFeeExpiredDate).ThenBy(a => a.MemberNo).ToList();
            }
            else if (sort == "Email")
            {
                sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(a => a.Email).ThenBy(a => a.MemberNo).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
                TempData["viewModel"] = memberInfos.AsQueryable().Where(a => a.MemberNo.HasValue).OrderBy(a => a.Email).ThenBy(a => a.MemberNo).ToList();
            }
            //var sortedMemberInfos = (memberInfos.AsQueryable().OrderBy(sort).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

			//TempData["Headers"] = GetStringList();
			ViewData["IsFromLocalHost"] = IsFromLocalHost();

			return View(sortedMemberInfos);
		}

		public string[][] GetStringList()
		{
			//List<string> temp = new List<string>();
			//temp.Add("MemberNo");
			//temp.Add("Name");
			//temp.Add("MemberFeeExpiredDate");
			//temp.Add("DateOfInitiation");
			//temp.Add("IDCardNo");
			//temp.Add("ContactNo");
			//temp.Add("Gender");

			string[][] tempList	 = new string[][]
			{
				new string[] {"MemberNo","Name", "Email", "MemberFeeExpiredDate", "DateOfInitiation", "IDCardNo", "ContactNo", "Gender"},
				//new string[] {"2.1","2.2", "2.3"},
				//new string[] {"3.1", "3.2", "3.3"}
			};
			
			return tempList;
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult GenerateExcel2()
		{
			var viewModel = (List<PublicMemberInfo>)TempData["viewModel"];
			return this.Excel(null, viewModel.AsQueryable(), "OrdinaryMemberInfo.xls", GetStringList());
		}

		[Authorize]
		public ActionResult Details(Guid? id)
		{
	
		    MembershipUser user = Membership.GetUser();
			if (id == null)
			{
				id = ((Guid)(user.ProviderUserKey));
			}
			else
			{
				user = Membership.GetUser(id);
			}

			PublicMemberInfo publicMemberInfo = GetPublicMemberInfo(id, user.UserName, user.Email);
			var viewModel = new OrdinaryMemberViewModel
			{
				PublicMemberInfo = publicMemberInfo,
			};

			if (IsFromLocalHost())
			{
				SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();
				if (_localEntities.OrdinaryMemberInfos.Any(a => a.IMemberID == id))
				{
					OrdinaryMemberInfo ordinaryMemberInfo = _localEntities.OrdinaryMemberInfos.Single(a => a.IMemberID == id);
					viewModel.OrdinaryMemberInfo = ordinaryMemberInfo;
					//var viewModel = new OrdinaryMemberViewModel
					//{
					//    OrdinaryMemberInfo = ordinaryMemberInfo,
					//    PublicMemberInfo = publicMemberInfo,
					//};

					if (ordinaryMemberInfo.MemberFeePayByID.HasValue)
					{
						ViewData["PayMethodName"] = _entities.PayMethods.SingleOrDefault(a => a.ID == ordinaryMemberInfo.MemberFeePayByID).Name;
					}
				}
			}

			ViewData["IsFromLocalHost"] = IsFromLocalHost();

		    return View(viewModel);
		}

		public bool IsFromLocalHost()
		{
			bool fromLocalHost = false;
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/SMCHSGManager");

            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0 && rootWebConfig.ConnectionStrings.ConnectionStrings["SMCHDBLocalEntities"] != null)
            {
                fromLocalHost = true;
            }
			return fromLocalHost;
		}

		private PublicMemberInfo GetPublicMemberInfo(Guid? id, string userName, string email)
		{
			PublicMemberInfo publicMemberInfo = new PublicMemberInfo();
			MemberInfo memberInfo = _entities.MemberInfos.Single(a => a.MemberID == id);
			publicMemberInfo.ContactNo = memberInfo.ContactNo;
			publicMemberInfo.DateOfInitiation = memberInfo.DateOfInitiation;
			publicMemberInfo.Email = email;
			publicMemberInfo.GenderID = memberInfo.GenderID;
			publicMemberInfo.Gender = memberInfo.Gender.Name;
			publicMemberInfo.ID = memberInfo.MemberID;
			publicMemberInfo.MemberNo = memberInfo.MemberNo;
			publicMemberInfo.Name = memberInfo.Name;
			publicMemberInfo.UserName = userName;
			publicMemberInfo.IsActive = memberInfo.IsActive;
			publicMemberInfo.IDCardNo = memberInfo.IDCardNo;
			publicMemberInfo.DateOfBirth = memberInfo.DateOfBirth;
			publicMemberInfo.CountryOfBirth = memberInfo.CountryOfBirth;
			publicMemberInfo.InitiateTypeID = memberInfo.InitiateTypeID;
			publicMemberInfo.InitiateType = memberInfo.InitiateType.Name;
			publicMemberInfo.PassportNo = memberInfo.PassportNo;
			publicMemberInfo.Remark = memberInfo.Remark;
			return publicMemberInfo;
		}

		//  POST: /Home/Create
		[Authorize(Roles = "Administrator")]
		 public ActionResult Create()
		 {
			 //SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();

		     var viewModel = new OrdinaryMemberViewModel
		     {
				 OrdinaryMemberInfo = new OrdinaryMemberInfo (),
			     PublicMemberInfo = new PublicMemberInfo (),
				 //Nationality = _localEntities.Nationalities.ToList(),
		         Gender = _entities.Genders.ToList(),
				 //Race = _localEntities.Races.ToList(),
				 //EmploymentStatus = _localEntities.EmploymentStatuses.ToList(),
		         PayMethod = _entities.PayMethods.ToList(),
				 InitiateType = _entities.InitiateTypes.Where(a => a.ID <=2).ToList(),
		     };

			 DefaultSettings(viewModel);

		     return View(viewModel);
		 }


		 //  POST: /Home/Create

		 //[HttpPost]
		 [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		 public ActionResult Create(OrdinaryMemberInfo OrdinaryMemberInfo, PublicMemberInfo PublicMemberInfo, FormCollection collection)
		 {
			 //SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();

			 AccountController ac = new AccountController ();
			 ac.InitializePublic(HttpContext.Request.RequestContext);

			 try
		     {
				 Guid memberID = Guid.Empty;
				 string errorString = ac.RegisterOrdinaryMember(PublicMemberInfo.Name, PublicMemberInfo.Email, ref memberID);
				 if (!string.IsNullOrEmpty(errorString))
				 {
					 ViewData["errorMsg"] = errorString;
					 throw new Exception();
				 }

				 MemberInfo memberInfo = GetMemberInfo(PublicMemberInfo, memberID);
				 _entities.AddToMemberInfos(memberInfo);
				 _entities.SaveChanges();

				 SetBackToNull(OrdinaryMemberInfo);

                 //OrdinaryMemberInfo.IMemberID = memberID;
                 //_localEntities.AddToOrdinaryMemberInfos(OrdinaryMemberInfo);
                 //_localEntities.SaveChanges();

				 MembershipUser newUser = Membership.GetUser(memberInfo.MemberID);
                 if (newUser.Email.Trim() != "password@smchsg.com")
                 {
                     HomeController hc = new HomeController();
                     try
                     {
                         hc.sendUserEmail(newUser);
                     }
                     catch
                     {
                     }
                 }
		         return RedirectToAction("Index");
		     }
		     catch
		     {
		         //Invalid - redisplay with errors
		         var viewModel = new OrdinaryMemberViewModel
		         {
		             OrdinaryMemberInfo = OrdinaryMemberInfo,
					 PublicMemberInfo = PublicMemberInfo,
					 //Nationality = _localEntities.Nationalities.ToList(),
		             Gender = _entities.Genders.ToList(),
					 //Race = _localEntities.Races.ToList(),
					 //EmploymentStatus = _localEntities.EmploymentStatuses.ToList(),
		             PayMethod = _entities.PayMethods.ToList(),
					 InitiateType = _entities.InitiateTypes.Where(a => a.ID <= 2).ToList(),
				 };

				 DefaultSettings(viewModel);

		          return View(viewModel);
		     }
		 }

		 private MemberInfo GetMemberInfo(PublicMemberInfo publicMemberInfo, Guid memberID)
		 {
			 MemberInfo memberInfo = new MemberInfo();
			 memberInfo.IsActive = true;
			 memberInfo.MemberID = memberID;

			 UpdateMemberInfoDetails(publicMemberInfo, memberInfo);

			 return memberInfo;
		 }

		 private void UpdateMemberInfo(PublicMemberInfo publicMemberInfo, MemberInfo memberInfo)
		 {
			 if (Roles.IsUserInRole("Administrator"))
			 {
				 memberInfo.IsActive = publicMemberInfo.IsActive;
			 }

			 UpdateMemberInfoDetails(publicMemberInfo, memberInfo);
		 }

		 private static void UpdateMemberInfoDetails(PublicMemberInfo publicMemberInfo, MemberInfo memberInfo)
		 {
			 if (!string.IsNullOrEmpty(publicMemberInfo.Name))
			 {
				 memberInfo.Name = publicMemberInfo.Name.Trim();
			 }
			 if (!string.IsNullOrEmpty(publicMemberInfo.ContactNo))
			 {
				 memberInfo.ContactNo = publicMemberInfo.ContactNo.Trim();
			 }
			 memberInfo.DateOfInitiation = publicMemberInfo.DateOfInitiation;
			 memberInfo.DateOfBirth = publicMemberInfo.DateOfBirth;
			 if (!string.IsNullOrEmpty(publicMemberInfo.CountryOfBirth))
			 {
				 memberInfo.CountryOfBirth = publicMemberInfo.CountryOfBirth;
			 }
			 memberInfo.GenderID = publicMemberInfo.GenderID;
			 if (!string.IsNullOrEmpty(publicMemberInfo.IDCardNo))
			 {
				 memberInfo.IDCardNo = publicMemberInfo.IDCardNo;
			 }
			 memberInfo.InitiateTypeID = publicMemberInfo.InitiateTypeID;
			 if (publicMemberInfo.MemberNo.HasValue)
			 {
				 memberInfo.MemberNo = publicMemberInfo.MemberNo;
			 }
			 memberInfo.PassportNo = publicMemberInfo.PassportNo;
			 memberInfo.Remark = publicMemberInfo.Remark;
		 }

        //
        // GET: /OrdinaryMember/Edit/5
		 [Authorize]
		 public ActionResult Edit(Guid id)
        {
			MembershipUser user = Membership.GetUser(id);
	
			PublicMemberInfo publicMemberInfo = GetPublicMemberInfo(id, user.UserName, user.Email);

			var viewModel = new OrdinaryMemberViewModel
				{
					PublicMemberInfo = publicMemberInfo,
					Gender = _entities.Genders.ToList(),
					PayMethod = _entities.PayMethods.ToList(),
					InitiateType = _entities.InitiateTypes.Where(a => a.ID <= 2).ToList(),
				};

            //if (IsFromLocalHost())
            //{
            //    SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();

            //    viewModel = new OrdinaryMemberViewModel
            //    {
            //        OrdinaryMemberInfo = _localEntities.OrdinaryMemberInfos.Single(a => a.IMemberID == id),
            //        PublicMemberInfo = publicMemberInfo,
            //        Nationality = _localEntities.Nationalities.ToList(),
            //        Gender = _entities.Genders.ToList(),
            //        Race = _localEntities.Races.ToList(),
            //        EmploymentStatus = _localEntities.EmploymentStatuses.ToList(),
            //        PayMethod = _entities.PayMethods.ToList(),
            //        InitiateType = _entities.InitiateTypes.Where(a => a.ID <= 2).ToList(),
            //    };

            //    DefaultSettings(viewModel);
            //}

			ViewData["IsFromLocalHost"] = IsFromLocalHost();

			return View(viewModel);
		}

		private static void DefaultSettings(OrdinaryMemberViewModel viewModel)
		{
			if (!viewModel.OrdinaryMemberInfo.NationalityID.HasValue)
			{
				viewModel.OrdinaryMemberInfo.NationalityID = 0;
			}
			if (!viewModel.OrdinaryMemberInfo.MemberFeePayByID.HasValue)
			{
				viewModel.OrdinaryMemberInfo.MemberFeePayByID = 0;
			}
			if (!viewModel.OrdinaryMemberInfo.RaceID.HasValue)
			{
				viewModel.OrdinaryMemberInfo.RaceID = 0;
			}
			if (!viewModel.OrdinaryMemberInfo.EmploymentStatusID.HasValue)
			{
				viewModel.OrdinaryMemberInfo.EmploymentStatusID = 0;
			}

			viewModel.PayMethod.Insert(0, new PayMethod());
            //viewModel.EmploymentStatus.Insert(0, new EmploymentStatus());
            //viewModel.Race.Insert(0, new Race());
            //viewModel.Nationality.Insert(0, new Nationality());
		}

		private static void ConvertZeroToNull(OrdinaryMemberInfo ordinaryMemberInfo)
		{
			if (!ordinaryMemberInfo.NationalityID.HasValue && ordinaryMemberInfo.NationalityID.Value == 0)
			{
				ordinaryMemberInfo.NationalityID = null;
			}
			if (!ordinaryMemberInfo.RaceID.HasValue && ordinaryMemberInfo.RaceID.Value == 0)
			{
				ordinaryMemberInfo.RaceID = null;
			}
			if (!ordinaryMemberInfo.EmploymentStatusID.HasValue && ordinaryMemberInfo.EmploymentStatusID.Value == 0)
			{
				ordinaryMemberInfo.EmploymentStatusID = null;
			}
			if (!ordinaryMemberInfo.MemberFeePayByID.HasValue && ordinaryMemberInfo.MemberFeePayByID.Value == 0)
			{
				ordinaryMemberInfo.MemberFeePayByID = null;
			}
		
		}
        //
        // POST: /OrdinaryMember/Edit/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(Guid id, PublicMemberInfo PublicMemberInfo, FormCollection collection)
        {

			MemberInfo memberInfo = _entities.MemberInfos.SingleOrDefault(a => a.MemberID == id);

            try
            {
				UpdateMemberInfo(PublicMemberInfo, memberInfo);

                //if (IsFromLocalHost())
                //{
                //    SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();
                //    OrdinaryMemberInfo ordinaryMemberInfo = _localEntities.OrdinaryMemberInfos.Single(a => a.IMemberID == id);
                //    UpdateModel(ordinaryMemberInfo, "OrdinaryMemberInfo");

                //    SetBackToNull(ordinaryMemberInfo);

                //    _localEntities.SaveChanges();
                //}

				_entities.SaveChanges();

				AccountController ac = new AccountController();
				ac.UpdateEmail(PublicMemberInfo.Email, id);

				if (Roles.IsUserInRole("Administrator"))
				{
					return RedirectToAction("Index");
				}
				else
				{
					return RedirectToAction("Details", new { id = id });
				}
            }
            catch
            {
                return View();
            }
        }

		private static void SetBackToNull(OrdinaryMemberInfo ordinaryMemberInfo)
		{
			if (ordinaryMemberInfo.MemberFeePayByID == 0)
			{
				ordinaryMemberInfo.MemberFeePayByID = null;
			}
			if (ordinaryMemberInfo.NationalityID == 0)
			{
				ordinaryMemberInfo.NationalityID = null;
			}
			if (ordinaryMemberInfo.RaceID == 0)
			{
				ordinaryMemberInfo.RaceID = null;
			}
			if (ordinaryMemberInfo.EmploymentStatusID == 0)
			{
				ordinaryMemberInfo.EmploymentStatusID = null;
			}
		}

		//
		// GET: /Home/Delete/5
		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(Guid id)
		{
			var memberInfo = _entities.MemberInfos.Single(a => a.MemberID == id);
			return View(memberInfo);
		}

		//
		// POST: /Home/Delete/5

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(Guid id, String confirmButton)
		{
			DeleteUser(id);
			//if (DeleteUser(id))
			//{
			//    ViewData["message"] = " was successfully deleted.";
			//}
			//else
			//{
			//    ViewData["message"] = " has been registered event or ordered product, can't be removed.  if you still need to remove, please contact administrator.";
			//}
			return View("Deleted");
		}

		public void DeleteUser(Guid id)
		{
			string userName = Membership.GetUser(id).UserName;

			if (!_entities.EventRegistrations.Any(a => a.MemberID == id) && !_entities.MemberOrders.Any(a => a.MemberID == id))
			{

				List<Guid> IDs = (from r in _entities.MemberInfos select r.MemberID).ToList();
				if (IDs.Contains(id))
				{
					var memberInfo = _entities.MemberInfos.Single(a => a.MemberID == id);

                    //SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();

                    //IDs = (from r in _localEntities.OrdinaryMemberInfos select r.IMemberID).ToList();
                    //if (IDs.Contains(id))
                    //{
                    //    var iMemberInfo = _localEntities.OrdinaryMemberInfos.Single(a => a.IMemberID == id);
                    //    _localEntities.DeleteObject(iMemberInfo);
                    //    _localEntities.SaveChanges();
                    //}

					_entities.DeleteObject(memberInfo);
					_entities.SaveChanges();
				}
			
				Membership.DeleteUser(userName, true);
				if (userName == Membership.GetUser().UserName)
				{
					AccountController ac = new AccountController();
					ac.FormsService = new FormsAuthenticationService();
					ac.FormsService.SignOut();
				}

				ViewData["message"] = userName + " was successfully deleted.";
			}
			else
			{
				ViewData["message"] = userName + " has been registered event or ordered product, can't be removed.  if you still need to remove, please contact administrator.";
			}

		}

		public ActionResult GetBirthDayFromLocalToServer()
		{
            //List<MemberInfo> memberInfos = _entities.MemberInfos.Where(a => a.MemberNo.HasValue && 
            //        a.MemberNo.Value < 999 && 
            //        a.MemberFeeExpiredDate.HasValue && 
            //        a.MemberFeeExpiredDate.Value > new DateTime(2011, 6, 1) &&
            //        a.MemberNo != 481 && 
            //        a.MemberNo != 339 && 
            //        a.MemberNo != 308).ToList();
            //foreach (MemberInfo mi in memberInfos)
            //{
            //    MemberFeePayment mfp = new MemberFeePayment();
            //    mfp.IMemberID = mi.MemberID;
            //    mfp.ToDate = mi.MemberFeeExpiredDate.Value;
            //    mfp.FromDate = mfp.ToDate.AddMonths(-6);
            //    mfp.PayMethodID = 1;

            //}


			SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();

			List<MemberInfo> memberInfo2s = _entities.MemberInfos.ToList();
			List<OrdinaryMemberInfo> ordinaryMembers = _localEntities.OrdinaryMemberInfos.ToList();

			foreach (OrdinaryMemberInfo omi in ordinaryMembers)
			{
				//if (!string.IsNullOrEmpty(omi.NRICOrFINNo) && omi.NRICOrFINNo.Substring(0, 1).ToLower() != "s")
				{
					MemberInfo mi = _entities.MemberInfos.SingleOrDefault(a => a.MemberID == omi.IMemberID);
					//mi.PassportNo = omi.NRICOrFINNo;
					mi.Remark = omi.Remark;
					_entities.SaveChanges();
				}
			}

			//SMCHSGManager.Helper.PenangAshramJob.ReadExistingExcel();

			return View();
		}


    }
}
