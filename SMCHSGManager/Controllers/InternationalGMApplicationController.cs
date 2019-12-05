using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Globalization;

namespace SMCHSGManager.Controllers
{
    public class InternationalGMApplicationController : Controller
    {
		private SMCHDBEntities _entities = new SMCHDBEntities();
		
		//
        // GET: /ApplicationInternationGM/

		[Authorize]
		public ActionResult Index(Guid? memberID, int? ashramID, int? applicationStatusID)
		{

			int allStatusID = _entities.ApplicationStatuses.Count() + 1;
			if (!applicationStatusID.HasValue)
			{
				applicationStatusID = allStatusID;
			}

			var viewModel = SMCHSGManager.Helper.PenangAshramJob.GetLatestInternationalApplicationStatus(applicationStatusID.Value, true);

            viewModel = viewModel.OrderByDescending(a => a.InternationalGMApplicationInfo.ApplyDate).ToList();

			if (memberID.HasValue)
			{
				viewModel = viewModel.Where(a => a.InternationalGMApplicationInfo.MemberID == memberID).OrderByDescending(a => a.InternationalGMApplicationInfo.ID).ToList();
			}

            int allID = _entities.AshramAndCenterInfos.Count() + 1;
			if (!ashramID.HasValue)
			{
				ashramID = allID;
			}
			if (ashramID != allID)
			{
				viewModel = viewModel.Where(a => a.InternationalGMApplicationInfo.AshramID == ashramID).OrderBy(a => a.InternationalGMApplicationInfo.ApplyDate).ToList();

				//viewModel1 = viewModel;
				//if (ashramID == 2)
				//{
				//    viewModel1 = viewModel.Where(a => a.AshramID == 2 || a.AshramID == 7 || a.AshramID == 8).OrderByDescending(a => a.ID).ToList();
				//}

			}

			ViewData["AshramAndCenterInfos"] = GetAshramInfosSelectList(ashramID.Value, allID);

			ViewData["applicationStatuses"] = GetApplicationStatusSelectList(applicationStatusID.Value, allStatusID);

			viewModel = viewModel.Where(a => a.InternationalGMApplicationInfo.ParentID.HasValue == false).ToList();

			return View(viewModel);
		}

		//private List<ApplicationPanangForm> GetApplicationExcelData(List<InternationalGMApplicationInfo> viewModel)
		//{
		//    List<ApplicationPanangForm> applicationPanangForms = new List<ApplicationPanangForm>();
		//    int i = 1;
		//    foreach (InternationalGMApplicationInfo aigm in viewModel)
		//    {
		//        ApplicationPanangForm applicationPanangForm = new ApplicationPanangForm();
		//        applicationPanangForm.No = i++;
		//        applicationPanangForm.ApplicationDate = DateTime.Now.ToUniversalTime() ;
		//        applicationPanangForm.ArrivalDate = aigm.ArrivalDate;
		//        applicationPanangForm.DepartureDate = aigm.DepartureDate;
		//        applicationPanangForm.AccomodationNeeded = aigm.AccommodationNeeded;

		//        applicationPanangForm.Center = aigm.MemberInfo.AshramAndCenterInfo.Name;
		//        applicationPanangForm.Country = aigm.MemberInfo.AshramAndCenterInfo.Country.Name;
		//        applicationPanangForm.Email = aigm.MemberInfo.AshramAndCenterInfo.Email;

		//        applicationPanangForm.DateOfInitiation = aigm.MemberInfo.DateOfInitiation;
		//        applicationPanangForm.Gender = aigm.MemberInfo.Gender.Name;
		//        applicationPanangForm.IDCardNo = aigm.MemberInfo.IDCardNo;
		//        applicationPanangForm.Name = aigm.MemberInfo.Name;
		//        applicationPanangForm.InitiateType = aigm.MemberInfo.InitiateType.Name;
		//        applicationPanangForm.DateOfBirth = aigm.MemberInfo.DateOfBirth;
		//        applicationPanangForm.CountryOfBirth = aigm.MemberInfo.CountryOfBirth;

		//        if (_entities.TransportInfos.Any(a => a.InternationalGMApplicationInfoID == aigm.ID))
		//        {
		//            TransportInfo transportInfo = _entities.TransportInfos.Single(a => a.InternationalGMApplicationInfoID == aigm.ID);
		//            applicationPanangForm.InBoundStationName = transportInfo.TransportStation.StationName;
		//            applicationPanangForm.InBoundFlightNo = transportInfo.ArrivalFlightNo;
		//            applicationPanangForm.InBoundDateTime = transportInfo.ArrivalDateTime;
		//            applicationPanangForm.OutBoundStationName = transportInfo.TransportStation1.StationName;
		//            applicationPanangForm.OutBoundFlightNo = transportInfo.DepartureFlightNo;
		//            applicationPanangForm.OutBoundDateTime = transportInfo.DepartureDateTime;
		//        }

		//        applicationPanangForms.Add(applicationPanangForm);
		//    }
		//    return applicationPanangForms;
		//}

		public List<SelectListItem> GetApplicationStatusSelectList(int initID, int allID)
		{

			List<SelectListItem> si = new List<SelectListItem>();
			if (initID == allID)
			{
				si.Add(new SelectListItem { Text = "All", Value = allID.ToString(), Selected = true });
			}
			else
			{
				si.Add(new SelectListItem { Text = "All", Value = allID.ToString() });
			}

			int j = 1;
			foreach (ApplicationStatus status in _entities.ApplicationStatuses.ToList())
			{
				SelectListItem item = new SelectListItem { Text = status.Name, Value = j.ToString() };
				if (initID == j)
				{
					item.Selected = true;
				}
				si.Add(item);
				j++;
			}
			return si;
		}

		public List<SelectListItem> GetAshramInfosSelectList(int initID, int allID)
		{
			//SMCHSGManager.Models.SMCHDBEntities _entities = new SMCHSGManager.Models.SMCHDBEntities();
			//int allID = _entities.AshramAndCenterInfos.Count() + 1;

			List<SelectListItem> si = new List<SelectListItem>();
			if (initID == allID)
			{
				si.Add(new SelectListItem { Text = "All", Value = allID.ToString(), Selected = true });
			}
			else
			{
				si.Add(new SelectListItem { Text = "All", Value = allID.ToString() });
			}

			int j = 1;
            // for PeNang only
			foreach (AshramAndCenterInfo status in _entities.AshramAndCenterInfos.ToList())
			{
				SelectListItem item = new SelectListItem { Text = status.Name, Value = j.ToString() };
				if (initID == j)
				{
					item.Selected = true;
				}
				si.Add(item);
				j++;
			}
			return si;
		}

		[Authorize]
		public ActionResult Details(int id)
		{
			InternationalGMApplicationInfo applicationInternationGMInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);

			ViewData["ApplicationStatus"] = _entities.InternationalGMApplicationStatus.Where(a => a.InternationalGMApplicationInfoID == id).OrderBy(a => a.ConfirmDate).ToList();
			GMAttendanceCountViewModel gmAttendanceViewModel = GetGMAttendanceMonthlyCounts(6, applicationInternationGMInfo.MemberInfo);
			ViewData["GMAttendanceViewModel"] = gmAttendanceViewModel;

			var viewModel = new InternationalGMApplicationViewModel
			{
				InternationalGMApplicationInfo = applicationInternationGMInfo,
				TransportInfos = applicationInternationGMInfo.InternationalGMApplicationTransportInfos.ToList(),
			};

			if (applicationInternationGMInfo.AshramAndCenterInfo.AccommodationPermit)
			{
				HsihuAshramEventModel hsihuAshramEventModel = new HsihuAshramEventModel();
				hsihuAshramEventModel.Remark = _entities.AshramAndCenterInfos.Single(a => a.ID == 2).Remark;

				List<InternationalGMApplicationInfo> allSubApplications = _entities.InternationalGMApplicationInfos.Where(a => a.ParentID == id).ToList();

				if (allSubApplications.Count > 0)
				{
					hsihuAshramEventModel.SundayGMs = allSubApplications.Where(a => a.ArrivalDate == a.DepartureDate).Select(a => a.ArrivalDate).ToList();
					hsihuAshramEventModel.TwoDaysRetreats = allSubApplications.Where(a => a.ArrivalDate != a.DepartureDate).Select(a => a.ArrivalDate).ToList();
				}
				viewModel.HsihuAshramEvent = hsihuAshramEventModel;
			}

			ViewData["SubmitDateTime"] = SMCHSGManager.Helper.PenangAshramJob.nextTrigger;

			return View(viewModel);

		}

	
		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "SuperAdmin")]
        // This is for reject only.
		public ActionResult Details(FormCollection collection, int id)
		{
			InternationalGMApplicationInfo internationalGMApplicationInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);

			int applicationStatusID = internationalGMApplicationInfo.InternationalGMApplicationStatus.OrderBy(a => a.ConfirmDate).LastOrDefault().ApplicationStatusID;
			int newApplicationStatusID = applicationStatusID;

			//string message = null;

			if (applicationStatusID == 1)
			{
				newApplicationStatusID = 3;
			}
			else if (applicationStatusID == 4)
			{
				newApplicationStatusID = 6;
			}


			//internationalGMApplicationInfo.Remark = collection.GetValues("Cp")[0];
			UpdateModel(internationalGMApplicationInfo, "InternationalGMApplicationInfo");
			_entities.SaveChanges();

			SMCHSGManager.Helper.PenangAshramJob.AddApplicationStatus(_entities, internationalGMApplicationInfo.ID, newApplicationStatusID);

			//message = "Your application has been rejected. " + internationalGMApplicationInfo.Remark;
            //message = internationalGMApplicationInfo.Remark;

 			string to = Membership.GetUser(internationalGMApplicationInfo.MemberID).Email;

            GenerateEmailMessageAndSend(internationalGMApplicationInfo, newApplicationStatusID);
	
			return RedirectToAction("Index", new { applicationStatusID = applicationStatusID });
	
		}

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Approve(int id)
        {
            InternationalGMApplicationInfo internationalGMApplicationInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);

            int applicationStatusID = internationalGMApplicationInfo.InternationalGMApplicationStatus.OrderBy(a => a.ConfirmDate).LastOrDefault().ApplicationStatusID;
            int newApplicationStatusID = applicationStatusID;

            if (applicationStatusID == 1)
            {
                newApplicationStatusID = 2;
            }
            else if (applicationStatusID == 4)
            {
                newApplicationStatusID = 5;
                GenerateEmailMessageAndSend(internationalGMApplicationInfo, newApplicationStatusID);
            }
            SMCHSGManager.Helper.PenangAshramJob.AddApplicationStatus(_entities, internationalGMApplicationInfo.ID, newApplicationStatusID);

            return RedirectToAction("Index", new { applicationStatusID = applicationStatusID });

        }

		//[Authorize(Roles = "Administrator")]
		//public ActionResult GenerateExcel2(int ashramID)
		//{
		//    string[][] tempList = null; //= new string[][];

		//    var viewModel = (List<ApplicationPanangForm>)TempData["viewModel"];
		//    if (ashramID == 3)
		//    {
		//        tempList = new string[1][]
		//        {
		//            new string[] {"No", "ApplicationDate",  "Email", "PenangReplyDate", "Country", "Center", "CountryOfBirth", "Center", "ArrivalDate", "DepartureDate", "IDCardNo", "Name", "Gender", "DateOfBirth", "DateOfInitiation", "InBoundStationName", "InBoundFlightNo", "InBoundDateTime", "OutBoundStationName", "OutBoundFlightNo", "OutBoundDateTime"},
		//        };
		//    }
		//    else if (ashramID == 2 || ashramID == 4)
		//    {
		//        tempList = new string[2][]
		//        {
		//            new string[] {"No", /*"Center",*/ "Country", "IDCardNo", "Name", "Gender", "DateOfInitiation", "DateOfBirth", "InitiateType", "AccomodationNeeded", "ArrivalDate", "DepartureDate", "Remark", },
		//            new string[] {"序號", /*"小中心",*/ "國家", "識別證號碼", "識別證姓名", "性別", "印心日", "出生日", "印全/半心",  "住宿否", "抵達时间", "离开时间", "備註",},
		//        };
		//    }
		//    string excelFileName = _entities.AshramAndCenterInfos.Single(a => a.ID == ashramID).Name;
		//    excelFileName = excelFileName.Trim();
		//    excelFileName = "Application2" + excelFileName + "GM.xls";

		//    return this.Excel(null, viewModel.AsQueryable(), excelFileName, tempList);
		//}

 		[Authorize]
		public ActionResult MyApplication()
        {
			MembershipUser user = Membership.GetUser();
			Guid memberID = ((Guid)(user.ProviderUserKey));
			return RedirectToAction("Index", new { memberID = memberID });
        }

		//
		// GET: /ApplicationInternationGM/Details/5

		[Authorize]
		public ActionResult Requirements(int ashramID)
		{

			MemberInfo iMemberInfo = GetCurrentMemberInfo();
            if(!iMemberInfo.DateOfBirth.HasValue || !iMemberInfo.DateOfInitiation.HasValue || string.IsNullOrEmpty(iMemberInfo.ContactNo)
                || string.IsNullOrEmpty(iMemberInfo.CountryOfBirth))
            {
				//return RedirectToAction("Edit", "MemberInfo", new { id = iMemberInfo.MemberID });
				return RedirectToAction("Edit", "OrdinaryMember", new { id = iMemberInfo.MemberID });
			}
            else
            {
                // Check if the age is more than 18 and small than 65
                string message = null;
                int age = SMCHSGManager.Helper.PenangAshramJob.GetAge(iMemberInfo.DateOfBirth.Value);
                if(age < 18)
                {
                    //if(age < 18)
                    //{
                        message += "your age is under 18 years old. ";
                    //}
					//else
					//{
					//    message += "You are over 65 years old. Please provide reason(s) for appeal.";
					//}
                }
 
                 // Check if the person ia red-card helder currently
				if(_entities.BlackListMembers.Any(a=>a.MemberD == iMemberInfo.MemberID && a.IDCardTypeID == 2 && a.DateTo > DateTime.Today) || _entities.BlackListMembers.Any(a=>a.MemberD == iMemberInfo.MemberID && a.IDCardTypeID == 2 && !a.DateTo.HasValue))
				{
				    message += "you are red ID card holder currently. ";
				}

                if(iMemberInfo.InitiateTypeID != 1)
                {
                    message += "you are not full-initiate. ";
                }

                ViewData["Message"] = message;
				ViewData["AshramID"] = ashramID;

                GMAttendanceCountViewModel gmAttendanceViewModel = GetGMAttendanceMonthlyCounts(6, iMemberInfo);

                 return View(gmAttendanceViewModel);

             }
		}

		private MemberInfo GetCurrentMemberInfo()
		{

			MembershipUser user = Membership.GetUser();
			Guid memberID = ((Guid)(user.ProviderUserKey));
			MemberInfo iMemberInfo = _entities.MemberInfos.Single(a => a.MemberID == memberID);

			return iMemberInfo;
		}


        public GMAttendanceCountViewModel GetGMAttendanceMonthlyCounts(int beforeMonths, MemberInfo iMemberInfo)
        {
            List<GMAttendanceMonthCount> monthCounts = new List<GMAttendanceMonthCount> ();

		    DateTime endDate = _entities.GroupMeditationAttendances.OrderByDescending(a => a.CheckInTime).Select(a => a.CheckInTime).First();
            int tCount = 0;
            bool meetRequire = true;
            for(int i=1; i <= beforeMonths; i++)
            {
               DateTime startDate = endDate.AddMonths(-1);
               int count =  _entities.GroupMeditationAttendances.Where(a=>a.CheckInTime > startDate && a.CheckInTime <= endDate
                           && a.MemberID == iMemberInfo.MemberID).Count();
               if(_entities.EventRegistrations.Any(a=>a.MemberID == iMemberInfo.MemberID && a.Event.StartDateTime >= startDate && a.Event.EndDateTime <= endDate /*&& a.SignTime.HasValue*/))
               {
                   count += _entities.EventRegistrations.Where(a => a.MemberID == iMemberInfo.MemberID && a.Event.StartDateTime >= startDate && a.Event.EndDateTime <= endDate /*&& a.SignTime.HasValue*/).Count();
               }

               GMAttendanceMonthCount monthCount = new GMAttendanceMonthCount();
               monthCount.FromDate = startDate.AddDays(1);
               monthCount.ToDate = endDate;
               monthCount.Count = count;
               monthCounts.Add(monthCount);

               tCount += count;
               if(count<2 && meetRequire)
               {
                   meetRequire = false;
               }
               endDate = startDate;
            }

            GMAttendanceCountViewModel gmAttendanceViewModel = new GMAttendanceCountViewModel();
            gmAttendanceViewModel.MemberInfo = iMemberInfo;
            gmAttendanceViewModel.GMAttendanceMonthCounts = monthCounts;
            gmAttendanceViewModel.TotalCount = tCount;
            gmAttendanceViewModel.MeetRequire = meetRequire;

            return gmAttendanceViewModel;
        }


        //
        // GET: /ApplicationInternationGM/Create

		[Authorize]
		public ActionResult Create(bool meetRequire, int ashramID)
		{
			int myCenterID = GetMyCenterID();

			var viewModel = new InternationalGMApplicationViewModel
			{
				InternationalGMApplicationInfo = new InternationalGMApplicationInfo(),
				//AshramAndCenterInfos = _entities.AshramAndCenterInfos.Where(a => a.ID != myCenterID).ToList(),
				AshramAndCenterInfos = _entities.AshramAndCenterInfos.Where(a => a.Name.StartsWith("Penang")).ToList(),
			};
			viewModel.InternationalGMApplicationInfo.ArrivalDate = DateTime.Today.AddDays(15);
			viewModel.InternationalGMApplicationInfo.DepartureDate = viewModel.InternationalGMApplicationInfo.ArrivalDate.AddDays(7);

			ViewData["ThirdSundays"] = ThirdSundayOfEachMonth(viewModel.InternationalGMApplicationInfo.DepartureDate);

			ViewData["MeetRequire"] = meetRequire;

			MemberInfo iMemberInfo = GetCurrentMemberInfo();
			int age = SMCHSGManager.Helper.PenangAshramJob.GetAge(iMemberInfo.DateOfBirth.Value);
			ViewData["65AgeAbove"] = false;
			if (age > 65)
			{
				ViewData["65AgeAbove"] = true;
			}

			return View(viewModel);
		}

		private int GetMyCenterID()
		{
			MembershipUser user = Membership.GetUser();
			Guid memberID = ((Guid)(user.ProviderUserKey));
			int myCenterID = _entities.MemberInfos.Single(a => a.MemberID == memberID).AshramAndCenterInfo.ID;
			return myCenterID;
		}

		//
		// POST: /ApplicationInternationGM/Create

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
		public ActionResult Create(bool meetRequire, int ashramID, FormCollection collection, InternationalGMApplicationInfo internationalGMApplicationInfo)
		{
			internationalGMApplicationInfo.AshramID = ashramID;
			MemberInfo iMemberInfo = GetCurrentMemberInfo();
			int age = SMCHSGManager.Helper.PenangAshramJob.GetAge(iMemberInfo.DateOfBirth.Value);
			ViewData["65AgeAbove"] = false;
			if (age > 65)
			{
				ViewData["65AgeAbove"] = true;
			}

			try
			{
				if (_entities.InternationalGMApplicationInfos.Any(a => a.MemberID == iMemberInfo.MemberID &&
                                                 (internationalGMApplicationInfo.ArrivalDate >= a.ArrivalDate && internationalGMApplicationInfo.DepartureDate <= a.DepartureDate ||
                                                 internationalGMApplicationInfo.ArrivalDate == a.DepartureDate || internationalGMApplicationInfo.DepartureDate == a.ArrivalDate)))
                                                 //(internationalGMApplicationInfo.ArrivalDate >= a.ArrivalDate && internationalGMApplicationInfo.ArrivalDate <= a.DepartureDate ||
                                                 //internationalGMApplicationInfo.DepartureDate >= a.ArrivalDate && internationalGMApplicationInfo.DepartureDate <= a.DepartureDate)))
				{
					//ModelState.AddModelError("InternationalGMApplicationInfo.ArrivalDate", "You already have application in this period!");
                    ModelState.AddModelError(string.Empty, "You already have application in this period!");
					throw new Exception();
				}

				VerifyInformation(meetRequire, internationalGMApplicationInfo, age);

				if (ashramID ==  3) //penang
				{
					return RedirectToAction("SelectTransport", new { ashramID = internationalGMApplicationInfo.AshramID, arrivalDate = internationalGMApplicationInfo.ArrivalDate, departureDate = internationalGMApplicationInfo.DepartureDate, remark = internationalGMApplicationInfo.Remark });
				}
				else 
				{
                    return RedirectToAction("HsihuAshramGM", new { ashramID = internationalGMApplicationInfo.AshramID, arrivalDate = internationalGMApplicationInfo.ArrivalDate, departureDate = internationalGMApplicationInfo.DepartureDate, remark = internationalGMApplicationInfo.Remark });
				}
			}
			catch
			{
				int myCenterID = GetMyCenterID();
				var viewModel = new InternationalGMApplicationViewModel
				{
					InternationalGMApplicationInfo = internationalGMApplicationInfo,
					//AshramAndCenterInfos = _entities.AshramAndCenterInfos.Where(a => a.ID != myCenterID).ToList(),
					AshramAndCenterInfos = _entities.AshramAndCenterInfos.Where(a => a.Name.StartsWith("Penang")).ToList(),
				};

				ViewData["MeetRequire"] = meetRequire;

				ViewData["ThirdSundays"] = ThirdSundayOfEachMonth(viewModel.InternationalGMApplicationInfo.DepartureDate);

				return View(viewModel);
			}
		
		}

		private void VerifyInformation(bool meetRequire, InternationalGMApplicationInfo internationalGMApplicationInfo, int age)
		{
			if (internationalGMApplicationInfo.ArrivalDate > internationalGMApplicationInfo.DepartureDate)
			{
                ModelState.AddModelError(string.Empty, "Departure date is later than arrival date. Pls check departure date."); //"DepartureDate should be more than ArrivalDate.");
				throw new Exception();
			}

            DateTime today = DateTime.Today.ToUniversalTime().AddHours(8);
            if (internationalGMApplicationInfo.ArrivalDate.AddDays(-7) < today)
			{
				//ModelState.AddModelError(string.Empty, "Too late to apply, please apply 7 dyas before arrival date at least.");
                ModelState.AddModelError(string.Empty, "Too late for application. Pls apply at least 7 days before departure date");
				throw new Exception();
			}

            if (internationalGMApplicationInfo.ArrivalDate.AddDays(-60) > today)
			{
				ModelState.AddModelError(string.Empty, "Too Early to apply, please apply 1 month before.");
				throw new Exception();
			}

			int? stayDays = 30; //_entities.AshramAndCenterInfos.Single(a => a.ID == internationalGMApplicationInfo.AshramID).MaxStayDays;
			if (stayDays.HasValue && internationalGMApplicationInfo.ArrivalDate.AddDays(stayDays.Value) < internationalGMApplicationInfo.DepartureDate)
			{
                //ModelState.AddModelError(string.Empty, "Application Period should be less than " + stayDays.Value.ToString() + " days.");
                ModelState.AddModelError(string.Empty, "Maximum stay is " + stayDays.Value.ToString() + " days.");
                throw new Exception();
			}

			DateTime thirdSunday = ThirdSundayOfEachMonth(internationalGMApplicationInfo.DepartureDate)[0];
			DateTime firstDay = new DateTime(thirdSunday.Year, thirdSunday.Month, 1);
			if (internationalGMApplicationInfo.AshramID == 2 && !(internationalGMApplicationInfo.ArrivalDate >= firstDay && internationalGMApplicationInfo.DepartureDate.Date <= thirdSunday))
			{
				ModelState.AddModelError(string.Empty, "Application Period should be between " + firstDay.ToString() + " To " + thirdSunday.ToString());
				throw new Exception();
			}

			if (string.IsNullOrEmpty(internationalGMApplicationInfo.Remark) && !meetRequire)
			{
				ModelState.AddModelError(string.Empty, "You do not fulfill that Minimum GM attendance is twice per month for the last 6 months. Please provide reason(s) for appeal in Remark field:");
				throw new Exception();
			}

			if (string.IsNullOrEmpty(internationalGMApplicationInfo.Remark) && age > 65)
			{
				ModelState.AddModelError(string.Empty, "You are over 65 years old. Please provide reason(s) for appeal in Remark field:");
				throw new Exception();
			}
		}

		public List<DateTime> ThirdSundayOfEachMonth(DateTime enddate)
		{
			List<DateTime> result = new List<DateTime>();

			int sundaymonthcount = 0;
			DateTime firstDayMonth = new DateTime(enddate.Year, enddate.Month, 1);

			for (DateTime traverser = firstDayMonth; traverser <= enddate.AddMonths(6); traverser = traverser.AddDays(1))
			{
				if (traverser.DayOfWeek == DayOfWeek.Sunday)
				{
					sundaymonthcount++;
					if (sundaymonthcount == 3)
					{
						result.Add(traverser);
						sundaymonthcount = 0;
						traverser = new DateTime(traverser.Year, traverser.Month, 1).AddMonths(1);
					}
				}
			}
			return result;
		}		

		//private static DateTime GetThirdSunday(DateTime dt)
		//{
		//    DateTime firstDayMonth = new DateTime(dt.Year, dt.Month, 1);
		//    DateTime lastDayMonth = firstDayMonth.AddMonths(1).AddDays(-1);

		//    DateTime requestDate = firstDayMonth;
		//    int sundaymonthcount = 0;
		//    for (DateTime traverser = firstDayMonth; traverser <= lastDayMonth; traverser = traverser.AddDays(1))
		//    {
		//        if (traverser.DayOfWeek == DayOfWeek.Sunday)
		//        {
		//            sundaymonthcount++;
		//            if (sundaymonthcount == 3)
		//            {
		//                requestDate = traverser;
		//                break;
		//            }
		//        }
		//    }
		//    return requestDate;
		//}


		private static List<DateTime> GetRetreatGMs(DateTime arrivalDate, DateTime departureDate)
		{
			DateTime[] retreatDates = new DateTime[12] 
			{
				new DateTime(2012, 1, 7), 
				new DateTime (2012, 2, 11),
				new DateTime(2012, 3, 10),
				new DateTime(2012, 4, 14),
				new DateTime(2012, 5, 12),
				new DateTime(2012, 6, 9),
				new DateTime(2012, 7, 14),
				new DateTime(2012, 8, 11),
				new DateTime(2012, 9, 8),
				new DateTime(2012, 10, 13),
				new DateTime(2012, 11, 10),
				new DateTime(2012, 12, 8),
			};

			List<DateTime> retreatDayStarts = new List<DateTime>();
			for(int i=0; i<retreatDates.Length; i++)
			{
				if (retreatDates[i] >= arrivalDate && retreatDates[i] <= departureDate)
				{
					retreatDayStarts.Add(retreatDates[i]);
				}
			}
	
			return retreatDayStarts;
		}

		private static List<DateTime> GetSundayGMs(DateTime arrivalDate, DateTime departureDate)
		{
			List<DateTime> sundayGMs = new List<DateTime>();
			int nextMonth = 0;
			DateTime firstSundayInMonth = GetFirstSundayInMonth(arrivalDate, nextMonth++);
			bool first = true;
			while (firstSundayInMonth <= departureDate)
			{
				if (firstSundayInMonth >= arrivalDate && firstSundayInMonth <= departureDate)
				{
					sundayGMs.Add(firstSundayInMonth);
				}
				if (first)
				{
					firstSundayInMonth = firstSundayInMonth.AddDays(14);
					first = false;
				}
				else
				{
					firstSundayInMonth = GetFirstSundayInMonth(arrivalDate, nextMonth++);
					first = true;
				}
			}

			return sundayGMs;
		}

		private static DateTime GetFirstSundayInMonth(DateTime arrivalDate, int nextMonth)
		{
			int nextMonthNo = arrivalDate.AddMonths(nextMonth).Month;
			int nextMonthYear = arrivalDate.AddMonths(nextMonth).Year;
			DateTime firstDayInMonth = new DateTime(nextMonthYear, nextMonthNo, 1);

			int diff = DayOfWeek.Sunday - firstDayInMonth.DayOfWeek;
			DateTime firstSundayInMonth = firstDayInMonth.AddDays(diff);
			return firstSundayInMonth;
		}

		[Authorize]
		public ActionResult HsihuAshramGM(int ashramID, DateTime arrivalDate, DateTime departureDate)
		{
			var viewModel = new HsihuAshramEventModel
			{
				 AshramID = ashramID,
				 Remark = _entities.AshramAndCenterInfos.Single(a => a.ID == ashramID).Remark,
			};
			if (ashramID == 2)
			{
				viewModel.SundayGMs = GetSundayGMs(arrivalDate, departureDate);
				viewModel.TwoDaysRetreats = GetRetreatGMs(arrivalDate, departureDate);
			}
			return View(viewModel);
		}

		[AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult HsihuAshramGM(int ashramID, DateTime arrivalDate, DateTime departureDate, string remark, FormCollection collection)
		{
			InternationalGMApplicationInfo internationalGMApplicationInfo = GetInternationalApplicationInfo(ashramID, arrivalDate, departureDate, remark);

			bool accommodationNeeded = bool.Parse(collection.GetValues("AccommodationNeededCheckBox")[0]);
			if (accommodationNeeded) //&& ashramID == 2 || ashramID == 4 || ashramID <= 7 || ashramID >= 9)
			{
				internationalGMApplicationInfo.AccommodationNeeded = accommodationNeeded;
			}
			_entities.AddToInternationalGMApplicationInfos(internationalGMApplicationInfo);
			_entities.SaveChanges();

			internationalGMApplicationInfo = _entities.InternationalGMApplicationInfos.OrderByDescending(a => a.ID).FirstOrDefault();

			int applicationStatusID = internationalGMApplicationInfo.InternationalGMApplicationStatus.LastOrDefault().ApplicationStatusID;

			List<string> strs = collection.AllKeys.ToList();
			foreach (string str in strs)
			{
				bool checkBox = bool.Parse(collection.GetValues(str)[0]);
				if (str.StartsWith("RetreatCheckBox") && checkBox)
				{
					DateTime dt = DateTime.Parse(str.Replace("RetreatCheckBox", ""));
					AddSubInternationalGMApplication(ashramID, internationalGMApplicationInfo.ID, applicationStatusID, dt, dt.AddDays(1));
				}
				else if (str.StartsWith("SundayGMCheckBox") && checkBox)
				{
					DateTime dt = DateTime.Parse(str.Replace("SundayGMCheckBox", ""));
					AddSubInternationalGMApplication(ashramID, internationalGMApplicationInfo.ID, applicationStatusID, dt, dt);
				}
			}

            GenerateEmailMessageAndSend(internationalGMApplicationInfo, internationalGMApplicationInfo.InternationalGMApplicationStatus.LastOrDefault().ApplicationStatusID);

			return RedirectToAction("index", new { memberID = internationalGMApplicationInfo.MemberID, applicationStatusID = 1 });
		}

		private void AddSubInternationalGMApplication(int ashramID, int parentID, int applicationStatusID, DateTime dtStart, DateTime dtEnd)
		{

			InternationalGMApplicationInfo retreatApplication = NewInternationalGMApplication(ashramID, dtStart, dtEnd, null, applicationStatusID);
			if (applicationStatusID == 2)
			{
				if (dtStart == dtEnd)
				{
					retreatApplication.Remark = "Attend Sunday Group Meditation on " + string.Format("{0: d MMM yyyy}", dtStart) + " at Hsihu Ashram";
				}
				else
				{
					retreatApplication.Remark = "Attend 2 days from " + string.Format("{0: d MMM yyyy}", dtStart) + "  retreat at Hsihu Ashram";
				}
			}
			if (parentID != 0)
			{
				retreatApplication.ParentID = parentID;
			}
			_entities.AddToInternationalGMApplicationInfos(retreatApplication);
			_entities.SaveChanges();
				
		}

		private static InternationalGMApplicationInfo NewInternationalGMApplication(int ashramID, DateTime arrivalDate, DateTime departureDate, string remark, int statusID)
		{
			InternationalGMApplicationInfo internationalGMApplicationInfo = new InternationalGMApplicationInfo();
			internationalGMApplicationInfo.AshramID = ashramID;
			internationalGMApplicationInfo.ArrivalDate = arrivalDate;
			internationalGMApplicationInfo.DepartureDate = departureDate;
			internationalGMApplicationInfo.ApplyDate = DateTime.Now.ToUniversalTime().AddHours(8);
            internationalGMApplicationInfo.Remark = remark;
			MembershipUser user = Membership.GetUser();
			Guid memberID = ((Guid)(user.ProviderUserKey));
			internationalGMApplicationInfo.MemberID = memberID;
			//internationalGMApplicationInfo.ApplicationStatusID = statusID;
			//internationalGMApplicationInfo.ConfirmDate = internationalGMApplicationInfo.ApplyDate;
			return internationalGMApplicationInfo;
		}


		public void GenerateEmailMessageAndSend(InternationalGMApplicationInfo internationalGMApplicationInfo, int applicationStatusID)
		{
			EmailMessage em = new EmailMessage();

			em.Subject = internationalGMApplicationInfo.MemberInfo.Gender.Name + ' ' + internationalGMApplicationInfo.MemberInfo.Name;

            string CPName = Roles.GetUsersInRole("Contact Person").FirstOrDefault();
            string CPEmail = Membership.GetUser(CPName).Email;
            string localCenterEmail = _entities.AshramAndCenterInfos.SingleOrDefault(a => a.ID == 1).Email; // "Singapore Center"

			//int? age;
			if (internationalGMApplicationInfo.MemberInfo.DateOfBirth.HasValue)
			{
				int age = SMCHSGManager.Helper.PenangAshramJob.GetAge(internationalGMApplicationInfo.MemberInfo.DateOfBirth.Value);
				em.Subject = em.Subject + " Age " + age.ToString();
			}
			em.Subject = em.Subject + ' ' + "Apply " + internationalGMApplicationInfo.AshramAndCenterInfo.Name + " GM (from " + string.Format("{0: d MMM yyyy}", internationalGMApplicationInfo.ArrivalDate) + " to " + string.Format("{0: d MMM yyyy}", internationalGMApplicationInfo.DepartureDate) + ")";

			em.Subject = em.Subject + ' ' + _entities.ApplicationStatuses.Single(a=>a.ID == applicationStatusID).Name;

            em.Message = "Dear Saint,\n\n";

            if (applicationStatusID == 2 || applicationStatusID == 5)
            {
                em.Message += "We approved your application.\n\n";
                em.Message += "Penang Centre has also acknowledged our submission and accepted it.\n";
                em.Message += "Please contact brother Meng Hock at his mobile +60-16-415-7068 should you have any queries concerning your pick-up.\n\n";

                em.Message += "Your co-operation is required with regards to the rules and regulations for a stay at Penang Ashram as we need to screen applicants well.\n\n";

                em.Message += "We hope that you also alert us if you are not feeling well prior to your trip there or leave the Ashram if you are feeling unwell during your stay there.\n\n";

                em.Message += "Have a good trip ahead.\n\n";
            }
            else if (applicationStatusID == 3 || applicationStatusID == 6)
            {
                em.Message += "Your application has been rejected. \r\nReason being " + internationalGMApplicationInfo.CPComments + "\n\n";
            }
            else if (applicationStatusID == 1)
            {
                AshramAndCenterInfo ashramAndCenterInfo = internationalGMApplicationInfo.MemberInfo.AshramAndCenterInfo;
                MembershipUser user = Membership.GetUser(internationalGMApplicationInfo.MemberID);

                em.Message += "Your application is received.\n\n";
                em.Message += "Please proceed with screening at Singapore Centre after group meditation session.";
                em.Message += "We require you to complete a health declaration form. Kindly allow us time to revert back while we process your application." + "\n\n";
            }

            em.Message += "Wishing you Master's Love & Blessings, \nSingapore Centre";

            em.From = "admin@smchsg.com";
            string to  = Membership.GetUser(internationalGMApplicationInfo.MemberID).Email;
            if (string.IsNullOrEmpty(to) || to.StartsWith("password") || !to.Contains('@'))
            {
                to = CPEmail;
            }
            em.To = to;

            em.cc = CPEmail + ", " + localCenterEmail;
            em.bcc = "admin@smchsg.com";

			EmailService es = new EmailService();
			es.SendMessage(em);
		}


		[Authorize]
		public ActionResult SelectTransport(int ashramID, DateTime arrivalDate, DateTime departureDate, string remark)
		{
			List<InternationalGMApplicationTransportInfo> transportInfos = GetTransportInfoList(ashramID, arrivalDate, departureDate);

			return View(transportInfos);
		}

		private static List<InternationalGMApplicationTransportInfo> NewTransportInfos(DateTime arrivalDate, DateTime departureDate, int arrivalInternationalTransportID, int departureInternationalTransportID)
		{
			List<InternationalGMApplicationTransportInfo> transportInfos = new List<InternationalGMApplicationTransportInfo>();
			InternationalGMApplicationTransportInfo transportInfo = NewTransportInfo(arrivalDate, true, arrivalInternationalTransportID, null);
			transportInfos.Add(transportInfo);

			transportInfo = NewTransportInfo(departureDate, false, departureInternationalTransportID, null);

			transportInfos.Add(transportInfo);
			return transportInfos;
		}

		private static InternationalGMApplicationTransportInfo NewTransportInfo(DateTime arrivalDate, bool inBound, int arrivalInternationalTransportID, string flightNo)
		{
			InternationalGMApplicationTransportInfo transportInfo = new InternationalGMApplicationTransportInfo();
			transportInfo.InternationalTransportID = arrivalInternationalTransportID;
			transportInfo.DateTime = arrivalDate;
			transportInfo.InBound = inBound;
			transportInfo.FlightNo = flightNo;
			return transportInfo;
		}

		public static List<SelectListItem> GetSelectListItem(int selectID, int? allID, int? emptyID, List<SelectListModel> selectLists)
		{

			//int i = 0;
			List<SelectListItem> si = new List<SelectListItem>();

			if (allID.HasValue)
			{
				if (selectID == allID.Value)
				{
					si.Add(new SelectListItem { Text = "All", Value = allID.Value.ToString(), Selected = true });
				}
				else
				{
					si.Add(new SelectListItem { Text = "All", Value = allID.Value.ToString() });
				}
				//i++;
			}

			if (emptyID.HasValue)
			{
				if (selectID == emptyID.Value)
				{
					si.Add(new SelectListItem { Text = null, Value = emptyID.Value.ToString(), Selected = true });
				}
				else
				{
					si.Add(new SelectListItem { Text = null, Value = emptyID.Value.ToString(), });
				}
				//i++;
			}

			foreach (SelectListModel selectItem in selectLists)
			{
				SelectListItem item = new SelectListItem { Text = selectItem.Name, Value = selectItem.ID.ToString() };
				if (selectID == selectItem.ID)
				{
					item.Selected = true;
				}
				si.Add(item);
			}
			//for(int j = 0; j < nameList.Length; j++)
			//{
			//    SelectListItem item = new SelectListItem { Text = nameList[j], Value = (j+i).ToString() };
			//    if (selectID == j+i)
			//    {
			//        item.Selected = true;
			//    }
			//    si.Add(item);
			//}
			
			return si;
		}

		//
		// POST: /ApplicationInternationGM/Create

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult SelectTransport(int ashramID, DateTime arrivalDate, DateTime departureDate, string remark, FormCollection collection)
		{
			InternationalGMApplicationInfo internationalGMApplicationInfo = GetInternationalApplicationInfo(ashramID, arrivalDate, departureDate, remark);
			try
			{
                AddNewTransportInfos(collection, internationalGMApplicationInfo, null);

				_entities.AddToInternationalGMApplicationInfos(internationalGMApplicationInfo);
				_entities.SaveChanges();

				internationalGMApplicationInfo = _entities.InternationalGMApplicationInfos.OrderByDescending(a => a.ID).FirstOrDefault();

                GenerateEmailMessageAndSend(internationalGMApplicationInfo, internationalGMApplicationInfo.InternationalGMApplicationStatus.LastOrDefault().ApplicationStatusID);

                ViewData["InternationalGMApplicationInfoID"] = internationalGMApplicationInfo.ID;

                return View("Done");
				//return RedirectToAction("index", new { memberID = internationalGMApplicationInfo.MemberID, applicationStatusID = 1 });
				//return RedirectToAction("Detail", new { id = internationalGMApplicationInfo.ID});
			}
			catch
			{
				List<InternationalGMApplicationTransportInfo> transportInfos = internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.ToList();
				InternationalTransportConfig(internationalGMApplicationInfo, transportInfos);
				return View(transportInfos);
			}
		}

		private List<InternationalGMApplicationTransportInfo> GetTransportInfoList(int ashramID, DateTime arrivalDate, DateTime departureDate)
		{
			int emptyID = _entities.InternationalTransports.OrderByDescending(a => a.ID).FirstOrDefault().ID + 2;

			List<SelectListModel> selectList = (from r in _entities.InternationalTransports
												where r.AshramID == ashramID
												orderby r.ID
												select new SelectListModel
												{
													ID = r.ID,
													Name = r.StationName,
												}).ToList();
			ViewData["InternationalTransports"] = GetSelectListItem(emptyID, null, emptyID, selectList);

			List<InternationalGMApplicationTransportInfo> transportInfos = NewTransportInfos(arrivalDate, departureDate, emptyID, emptyID);

			return transportInfos;

		}

		//private void AddNewTransportInfos(FormCollection collection, InternationalGMApplicationInfo internationalGMApplicationInfo, string hearder)
		//{
		//    int emptyID = _entities.InternationalTransports.OrderByDescending(a => a.ID).FirstOrDefault().ID + 2;
		//    for (int i = 0; i < 2; i++)
		//    {//"TransportInfos." TransportInfos.FlightNo
		//        string collectionKey = hearder + "InternationalTransportID";
		//        int internationalTransportID = int.Parse(collection.GetValues(collectionKey)[i]);
		//        collectionKey = hearder + "DateTime";
		//        if (internationalTransportID != emptyID && collection.GetValues(collectionKey)[i] != null)
		//        {
		//            DateTime dt = DateTime.Parse(collection.GetValues(collectionKey)[i]);
		//            collectionKey = hearder + "FlightNo";
		//            string flightNo = null;
		//            if (collection.GetValues(collectionKey) != null)
		//            {
		//                flightNo = collection.GetValues(collectionKey)[i];
		//            }
		//            bool inBound = true;
		//            if (i != 0) inBound = false;
		//            InternationalGMApplicationTransportInfo transportInfo = NewTransportInfo(dt, inBound, internationalTransportID, flightNo);
		//            internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.Add(transportInfo);
		//        }
		//    }
		//}

		private void AddNewTransportInfos(FormCollection collection, InternationalGMApplicationInfo internationalGMApplicationInfo, string hearder)
		{
            int ret = 0;
			int emptyID = _entities.InternationalTransports.OrderByDescending(a => a.ID).FirstOrDefault().ID + 2;

			string transportIDStr1 = collection.GetValues(hearder + "InternationalTransportIDInBound")[0];
			string dtStr = string.Format("{0:d MMM yyyy} ", internationalGMApplicationInfo.ArrivalDate) +  collection.GetValues(hearder + "TimeInBound")[0];
			string flightNoStr1 = collection.GetValues(hearder + "FlightNoInBound")[0];

			bool ret1 = EditTransportInfo(transportIDStr1, dtStr, flightNoStr1, true, internationalGMApplicationInfo, emptyID);

			string transportIDStr2 = collection.GetValues(hearder + "InternationalTransportIDOutBound")[0];
			dtStr = string.Format("{0:d MMM yyyy} ", internationalGMApplicationInfo.DepartureDate) + collection.GetValues(hearder + "TimeOutBound")[0];
			string flightNoStr2 = collection.GetValues(hearder + "FlightNoOutBound")[0];

			bool ret2 = EditTransportInfo(transportIDStr2, dtStr, flightNoStr2, false, internationalGMApplicationInfo, emptyID);

            if (!ret1 || !ret2)
            {
                ret = 1;
                if (internationalGMApplicationInfo.ArrivalDate > internationalGMApplicationInfo.DepartureDate)
                {
                    ret = 2;
                }
            }

            if (ret == 1)
            {
                ModelState.AddModelError(string.Empty, "you input wrong time, please correct!");
                throw new Exception();
            }
            else if (ret == 2)
            {
                VerifyInformation(true, internationalGMApplicationInfo, 0);
            }
            else if (transportIDStr1 == "1" && string.IsNullOrWhiteSpace(flightNoStr1) || transportIDStr2 == "1" && string.IsNullOrWhiteSpace(flightNoStr2))
            {
                ModelState.AddModelError(string.Empty, "please input your flight no!");
                throw new Exception();
            }

		}

		private bool EditTransportInfo(string transportIDStr, string dtStr, string flightNo, bool inBound, InternationalGMApplicationInfo internationalGMApplicationInfo, int emptyID)
		{
            bool ret = true;
            bool exist = internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.Any(a => a.InBound == inBound);
			
            int internationalTransportID = int.Parse(transportIDStr);

            if (internationalTransportID != 1)
            {
                flightNo = null;
            }

            if (internationalTransportID == emptyID)
            {
                if (exist)
                {
                    InternationalGMApplicationTransportInfo transportInfo = internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.SingleOrDefault(a => a.InBound == inBound);
                    _entities.DeleteObject(transportInfo); 
                    _entities.SaveChanges();
                }
            }
            else
			{

                DateTime dt = new DateTime();
                if (DateTime.TryParse(dtStr, out dt))
                {
                    if (inBound)
                    {
                        internationalGMApplicationInfo.ArrivalDate = dt;
                    }
                    else
                    {
                        internationalGMApplicationInfo.DepartureDate = dt;
                    }
                }
                else
                {
                    ret = false;
                }
                
                if (exist)
                {
                    InternationalGMApplicationTransportInfo transportInfo = internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.SingleOrDefault(a => a.InBound == inBound);
                    transportInfo.InternationalTransportID = internationalTransportID;
                    transportInfo.FlightNo = flightNo;
                    if(ret)
                    {
                        transportInfo.DateTime = dt;
                    }
                    UpdateModel(transportInfo, "InternationalGMApplicationTransportInfo");
                    _entities.SaveChanges();
                }
                else
                {
                    InternationalGMApplicationTransportInfo transportInfo = NewTransportInfo(dt, inBound, internationalTransportID, flightNo);
                    internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.Add(transportInfo);
                } 
                
 			}

			return ret;
		}


        private InternationalGMApplicationInfo GetInternationalApplicationInfo(int ashramID, DateTime arrivalDate, DateTime departureDate, string remark)
		{
			InternationalGMApplicationInfo internationalGMApplicationInfo = NewInternationalGMApplication(ashramID, arrivalDate, departureDate, remark, 1);

			InternationalGMApplicationStatu InternationalGMApplicationStatus = new InternationalGMApplicationStatu();
            InternationalGMApplicationStatus.ApplicationStatusID = 1;// Screening(internationalGMApplicationInfo);
			InternationalGMApplicationStatus.ConfirmDate = DateTime.Now.ToUniversalTime().AddHours(8);

			internationalGMApplicationInfo.InternationalGMApplicationStatus.Add(InternationalGMApplicationStatus);

			return internationalGMApplicationInfo;
		}
	
		//
        // GET: /ApplicationInternationGM/Edit/5

		[Authorize]
		public ActionResult Edit(int id)
        {
			InternationalGMApplicationInfo applicationInternationGMInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);

			int emptyID = _entities.InternationalTransports.OrderByDescending(a => a.ID).FirstOrDefault().ID + 2;

			List<InternationalGMApplicationTransportInfo> transportInfos = NewTransportInfos(applicationInternationGMInfo.ArrivalDate, applicationInternationGMInfo.DepartureDate, emptyID, emptyID); 
			if (applicationInternationGMInfo.InternationalGMApplicationTransportInfos.Count > 0)
			{
				transportInfos = applicationInternationGMInfo.InternationalGMApplicationTransportInfos.ToList();
			}
			var viewModel = new InternationalGMApplicationViewModel
			{
				InternationalGMApplicationInfo = applicationInternationGMInfo,
				TransportInfos = transportInfos,
			};
			
			if (_entities.InternationalTransports.Any(a => a.AshramID == applicationInternationGMInfo.AshramID))
			{
				InternationalTransportConfig(applicationInternationGMInfo, viewModel.TransportInfos);
			}

			HsiHuAshramConfig(id, applicationInternationGMInfo, viewModel);

			return View(viewModel);
		}

		private void HsiHuAshramConfig(int id, InternationalGMApplicationInfo applicationInternationGMInfo, InternationalGMApplicationViewModel viewModel)
		{
			if (applicationInternationGMInfo.AshramAndCenterInfo.AccommodationPermit)
			{
				HsihuAshramEventModel hsihuAshramEventModel = new HsihuAshramEventModel();
				hsihuAshramEventModel.Remark = _entities.AshramAndCenterInfos.Single(a => a.ID == 2).Remark;

				if (applicationInternationGMInfo.AshramID == 2)
				{
					List<DateTime> sundayGMs = GetSundayGMs(applicationInternationGMInfo.ArrivalDate, applicationInternationGMInfo.DepartureDate);
					List<DateTime> twoDaysRetreats = GetRetreatGMs(applicationInternationGMInfo.ArrivalDate, applicationInternationGMInfo.DepartureDate);

					hsihuAshramEventModel.SundayGMs = sundayGMs;
					hsihuAshramEventModel.TwoDaysRetreats = twoDaysRetreats;
					hsihuAshramEventModel.TwoDaysRetreatCheckeds = GetCheckeds(id, twoDaysRetreats.Count, twoDaysRetreats);
					hsihuAshramEventModel.SundayGMCheckeds = GetCheckeds(id, sundayGMs.Count, sundayGMs);
				}
				viewModel.HsihuAshramEvent = hsihuAshramEventModel;
			}

			ViewData["ThirdSundays"] = ThirdSundayOfEachMonth(viewModel.InternationalGMApplicationInfo.DepartureDate);
		}

		private InternationalGMApplicationViewModel NewInternationalGMApplication(int id)
		{
			InternationalGMApplicationInfo applicationInternationGMInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);
			var viewModel = new InternationalGMApplicationViewModel
			{
				InternationalGMApplicationInfo = applicationInternationGMInfo,
				TransportInfos = applicationInternationGMInfo.InternationalGMApplicationTransportInfos.ToList(),
			};

			if (applicationInternationGMInfo.AshramAndCenterInfo.AccommodationPermit)
			{
				HsihuAshramEventModel hsihuAshramEventModel = new HsihuAshramEventModel();
				hsihuAshramEventModel.Remark = _entities.AshramAndCenterInfos.Single(a => a.ID == 2).Remark;

				if (applicationInternationGMInfo.AshramID == 2)
				{
					List<DateTime> sundayGMs = GetSundayGMs(applicationInternationGMInfo.ArrivalDate, applicationInternationGMInfo.DepartureDate);
					List<DateTime> twoDaysRetreats = GetRetreatGMs(applicationInternationGMInfo.ArrivalDate, applicationInternationGMInfo.DepartureDate);

					hsihuAshramEventModel.SundayGMs = sundayGMs;
					hsihuAshramEventModel.TwoDaysRetreats = twoDaysRetreats;
					hsihuAshramEventModel.TwoDaysRetreatCheckeds = GetCheckeds(id, twoDaysRetreats.Count, twoDaysRetreats);
					hsihuAshramEventModel.SundayGMCheckeds = GetCheckeds(id, sundayGMs.Count, sundayGMs);
				}
				viewModel.HsihuAshramEvent = hsihuAshramEventModel;
			}

			if (_entities.InternationalTransports.Any(a => a.AshramID == applicationInternationGMInfo.AshramID))
			{
				InternationalTransportConfig(applicationInternationGMInfo, viewModel.TransportInfos);
			}

			ViewData["ThirdSundays"] = ThirdSundayOfEachMonth(viewModel.InternationalGMApplicationInfo.DepartureDate);

			return viewModel;

		}

		private void InternationalTransportConfig(InternationalGMApplicationInfo applicationInternationGMInfo, List<InternationalGMApplicationTransportInfo> transportInfos)
		{
			int emptyID = _entities.InternationalTransports.OrderByDescending(a => a.ID).FirstOrDefault().ID + 2;

			List<SelectListModel> selectList = (from r in _entities.InternationalTransports
												where r.AshramID == applicationInternationGMInfo.AshramID
												orderby r.ID
												select new SelectListModel
												{
													ID = r.ID,
													Name = r.StationName,
												}).ToList();

			ViewData["InternationalTransports"] = GetSelectListItem(emptyID, null, emptyID, selectList);

			if (transportInfos.Count() == 0)
			{
				transportInfos = NewTransportInfos(applicationInternationGMInfo.ArrivalDate,
					applicationInternationGMInfo.DepartureDate, emptyID, emptyID);
			}
			else if (transportInfos.Count() == 1)
			{
				InternationalGMApplicationTransportInfo transportInfo;
				if (transportInfos[0].InBound)
				{
					transportInfo = NewTransportInfo(applicationInternationGMInfo.DepartureDate, false, emptyID, null);
					ViewData["InBoudInternationalTransports"] = GetSelectListItem(transportInfos.Single(a => a.InBound).InternationalTransportID, null, emptyID, selectList);
				}
				else
				{
					transportInfo = NewTransportInfo(applicationInternationGMInfo.ArrivalDate, true, emptyID, null);
					ViewData["OutBoudInternationalTransports"] = GetSelectListItem(transportInfos[0].InternationalTransportID, null, emptyID, selectList);
				}
				transportInfos.Add(transportInfo);
			}
			else
			{
				ViewData["InBoudInternationalTransports"] = GetSelectListItem(transportInfos.SingleOrDefault(a => a.InBound).InternationalTransportID, null, emptyID, selectList);
				ViewData["OutBoudInternationalTransports"] = GetSelectListItem(transportInfos.SingleOrDefault(a => !a.InBound).InternationalTransportID, null, emptyID, selectList);
			}
		}


		private List<bool> GetCheckeds(int id, int count, List<DateTime> twoDaysRetreats)
		{
			List<DateTime> allSubApplicationDateTimes = _entities.InternationalGMApplicationInfos.Where(a => a.ParentID == id).Select(a => a.ArrivalDate).ToList();
			List<bool> twoDaysRetreatCheckeds = new List<bool>(count);
			for (int i = 0; i < count; i++)
			{
				if (allSubApplicationDateTimes.Contains(twoDaysRetreats[i]))
				{
					twoDaysRetreatCheckeds[i] = true;
				}
			}
			return twoDaysRetreatCheckeds;
		}

        //
        // POST: /ApplicationInternationGM/Edit/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize]
		public ActionResult Edit(int id, FormCollection collection)
        {
			InternationalGMApplicationInfo internationalGMApplicationInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);
			//MemberInfo iMemberInfo = GetCurrentMemberInfo();
			int age = SMCHSGManager.Helper.PenangAshramJob.GetAge(internationalGMApplicationInfo.MemberInfo.DateOfBirth.Value);
			GMAttendanceCountViewModel gmAttendanceViewModel = GetGMAttendanceMonthlyCounts(6, internationalGMApplicationInfo.MemberInfo);
			bool meetRequire = gmAttendanceViewModel.MeetRequire;

			try
            {
                //var oldTransportInfos = _entities.InternationalGMApplicationTransportInfos.Where(a => a.InternationalGMApplicationInfoID == id).ToList();
                //foreach (var transportInfo in oldTransportInfos)
                //{
                //    _entities.DeleteObject(transportInfo);
                //}
                //_entities.SaveChanges();

				UpdateModel(internationalGMApplicationInfo, "InternationalGMApplicationInfo");
				VerifyInformation(meetRequire, internationalGMApplicationInfo, age);

                AddNewTransportInfos(collection, internationalGMApplicationInfo, "TransportInfos.");
				_entities.SaveChanges();

				return RedirectToAction("Details", new { id = id });
            }
            catch
            {
				var viewModel = new InternationalGMApplicationViewModel
				{
					InternationalGMApplicationInfo = internationalGMApplicationInfo,
					TransportInfos = internationalGMApplicationInfo.InternationalGMApplicationTransportInfos.ToList(),
				};
				if (_entities.InternationalTransports.Any(a => a.AshramID == internationalGMApplicationInfo.AshramID))
				{
					InternationalTransportConfig(internationalGMApplicationInfo, viewModel.TransportInfos);
				}
				HsiHuAshramConfig(id, internationalGMApplicationInfo, viewModel);

				return View(viewModel);
			}
        }

        //
        // GET: /ApplicationInternationGM/Delete/5

		[Authorize]
		public ActionResult Delete(int id)
        {
			InternationalGMApplicationInfo applicationInternationGMInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);
			return View(applicationInternationGMInfo);
        }

        //
        // POST: /ApplicationInternationGM/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
		public ActionResult Delete(int id, FormCollection collection)
        {
			if (id != 0)
			{
				InternationalGMApplicationInfo applicationInternationGMInfo = _entities.InternationalGMApplicationInfos.Single(a => a.ID == id);
				List<InternationalGMApplicationInfo> allSubApplications = _entities.InternationalGMApplicationInfos.Where(a => a.ParentID == id).ToList();

				if (allSubApplications.Count > 0)
				{
					foreach (InternationalGMApplicationInfo subApplication in allSubApplications)
					{
						_entities.DeleteObject(subApplication);
					}
				}
				_entities.DeleteObject(applicationInternationGMInfo);
				_entities.SaveChanges();

				//SendEmailToLocalCP(applicationInternationGMInfo, 1);
			}
			if (!Roles.IsUserInRole("SuperAdmin"))
			{
				MembershipUser user = Membership.GetUser();
				ViewData["MemberID"] = (Guid)(user.ProviderUserKey);
			}

			

			return View("Deleted");
        }


		public ActionResult SubmitToDestinationAshram(int ashramID)
		{
			SMCHSGManager.Helper.PenangAshramJob.SaveContentAndSendEmail(ashramID);
			return View();
		}

    }
}
