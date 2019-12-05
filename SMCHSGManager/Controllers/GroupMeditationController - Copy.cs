using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Globalization;
using System.Web.Security;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace SMCHSGManager.Controllers
{
    public class GroupMeditationController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();

        //
        // GET: /GroupMeditation/
        // This is different format from EventController index...
		[Authorize]
		public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
  			if (!endDate.HasValue)
			{
				endDate = DateTime.UtcNow.AddHours(8);
			}

            DateTime latestGroupMeditationAttendanceDB = DateTime.Today; // _entities.GroupMeditationAttendances.OrderByDescending(a => a.CheckInTime).Select(a => a.CheckInTime).FirstOrDefault();

            latestGroupMeditationAttendanceDB = latestGroupMeditationAttendanceDB.AddDays(1);

            var events = (from r in _entities.GroupMeditations 
                         where 	 //r.EndDateTime <= today && 
										(startDate == null || r.StartDateTime >= startDate.Value)
									  && (endDate == null || r.EndDateTime <= endDate.Value) && r.EndDateTime < latestGroupMeditationAttendanceDB
                                      && r.InitiateTypeID == 1
                         orderby r.StartDateTime descending select r).ToList();

             return View(events);
        }

        //
        // GET: /GroupMeditation/Details/5
        // This will show event register information, not event details.....
		[Authorize]
		public ActionResult Details(int eventID, bool descending)
        {
  
            var viewModel = (from r in _entities.GroupMeditationAttendances
                              where r.GroupMeditationID == eventID && r.CheckInTime != null 
                             orderby r.CheckInTime
                             select r).ToList();

			if (descending)
			{
				viewModel = viewModel.OrderByDescending(a => a.CheckInTime).ToList();
			}

			GroupMeditation aEvent = _entities.GroupMeditations.Single(a => a.ID == eventID);
            ViewData["aEvent"] = aEvent;
            ViewData["Descending"] = descending;
   
            return View(viewModel);
        }


        //
        // GET: /GroupMeditation/Create

        //[Authorize(Roles = "SuperAdmin")]
        //public ActionResult CreateGMEvent(DateTime? startDate, DateTime? endDate)
        //{
        //    int year = DateTime.Today.Year;// +1;
        //    //List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where r.StartDateTime.Year == nextYear orderby r.StartDateTime select r).ToList();
        //    if (!startDate.HasValue)
        //    {
        //        startDate = new DateTime(year, 1, 1);
        //    }

        //    if (!endDate.HasValue)
        //    {
        //        endDate = new DateTime(year, 12, 31);
        //    }

        //    for (DateTime dt = startDate.Value; dt <= endDate; dt = dt.AddDays(1))
        //    {
        //        switch (dt.DayOfWeek)
        //        {
        //            case DayOfWeek.Sunday:
        //                AddGroupMeditationItem(dt, 9, 3, 1);
        //               break;
        //            case DayOfWeek.Monday:
        //                AddGroupMeditationItem(dt, 19.5, 2, 1);
        //                break;
        //            case DayOfWeek.Tuesday:
        //                AddGroupMeditationItem(dt, 19.5, 2, 1);
        //                break;
        //            case DayOfWeek.Wednesday:
        //                AddGroupMeditationItem(dt, 23, 7, 1);
        //                break;
        //            case DayOfWeek.Thursday:
        //                AddGroupMeditationItem(dt, 19.5, 2, 1);
        //                break;
        //            case DayOfWeek.Friday:
        //                AddGroupMeditationItem(dt, 20, 0.5, 2);
        //                break;
        //            case DayOfWeek.Saturday:
        //                AddGroupMeditationItem(dt, 9, 3, 1);
        //                AddGroupMeditationItem(dt, 19, 2.5, 1);
        //                AddGroupMeditationItem(dt, 23, 8, 1);
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return View();
        //}

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult CreateNextYearGMEvent(DateTime? startDate, DateTime? endDate)
        {
            int year = DateTime.Today.Year +1;
            //List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where r.StartDateTime.Year == nextYear orderby r.StartDateTime select r).ToList();
            if (!startDate.HasValue)
            {
                startDate = new DateTime(year, 1, 1);
            }

            if (!endDate.HasValue)
            {
                endDate = new DateTime(year, 12, 31);
            }

            for (DateTime dt = startDate.Value; dt <= endDate; dt = dt.AddDays(1))
            {
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        AddGroupMeditationItem(dt, 9, 3, 1);
                        break;
                    case DayOfWeek.Monday:
                        AddGroupMeditationItem(dt, 19.5, 2, 1);
                        break;
                    case DayOfWeek.Tuesday:
                        AddGroupMeditationItem(dt, 19.5, 2, 1);
                        break;
                    case DayOfWeek.Wednesday:
                        AddGroupMeditationItem(dt, 23, 7, 1);
                        break;
                    case DayOfWeek.Thursday:
                        AddGroupMeditationItem(dt, 19.5, 2, 1);
                        break;
                    case DayOfWeek.Friday:
                        AddGroupMeditationItem(dt, 20, 0.5, 3);
                        break;
                    case DayOfWeek.Saturday:
                        AddGroupMeditationItem(dt, 9, 3, 1);
                        AddGroupMeditationItem(dt, 19, 2.5, 1);
                        AddGroupMeditationItem(dt, 23, 8, 1);
                        break;
                    default:
                        break;
                }
            }

            return View();
        }

        private void AddGroupMeditationItem(DateTime dt, double startTime, double duration, int InitiateTypeID)
        {
            GroupMeditation gm = new GroupMeditation();
            gm.InitiateTypeID = InitiateTypeID;
            gm.StartDateTime = dt.Date.AddHours(startTime);
            gm.EndDateTime = gm.StartDateTime.AddHours(duration);

            if (!_entities.GroupMeditations.Any(a => a.StartDateTime == gm.StartDateTime))
            {
                _entities.AddToGroupMeditations(gm);
                _entities.SaveChanges();
            }
            
        }



		[Authorize(Roles = "SuperAdmin")]
		public ActionResult CorrectGMEvent()
		{
            
            //var latestMemberFeePayments = (from r in _entities.MemberFeePayments
            //                               orderby r.ToDate descending
            //                               group r by r.MemberInfo.MemberID into h
            //                               select new
            //                               {
            //                                   MemberID = h.Key,
            //                                   ToDate = h.Max(a => a.ToDate),
            //                               }).ToList();

            //foreach (var mfp in latestMemberFeePayments)
            //{
            //    MemberInfo memberInfo = _entities.MemberInfos.Single(a => a.MemberID == mfp.MemberID);
            //    if (memberInfo.MemberFeeExpiredDate < mfp.ToDate)
            //    {
            //        memberInfo.MemberFeeExpiredDate = mfp.ToDate;
            //        _entities.SaveChanges();
            //    }
            // }

            //List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where (r.InitiateTypeID == 3 || r.StartDateTime.Hour == 9 && r.StartDateTime > new DateTime(2012, 2, 6)) orderby r.StartDateTime select r).ToList();

            //foreach (GroupMeditation gm in GMs)
            //{
            //    if (gm.StartDateTime.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        gm.EndDateTime = gm.EndDateTime.AddHours(1);
            //    }
            //    else if (gm.StartDateTime.DayOfWeek == DayOfWeek.Friday)
            //    {
            //        gm.StartDateTime = gm.StartDateTime.AddHours(0.5);
            //        gm.EndDateTime = gm.EndDateTime.AddHours(0.5);
            //    }
            //    _entities.SaveChanges();
            //}
            List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where
                                             r.StartDateTime.Hour == 9 
                                         orderby r.StartDateTime select r).ToList();

            foreach (GroupMeditation gm in GMs)
            {
                //if ( gm.StartDateTime.DayOfWeek == DayOfWeek.Sunday )
                //{
                //    if( gm.StartDateTime > new DateTime(2012, 2, 6)) 
                //    {
                //        gm.EndDateTime = gm.StartDateTime.AddHours(4);
                //    }
                //    else
                //    {
                //        gm.EndDateTime = gm.StartDateTime.AddHours(3);
                //    }
                //}
                if  (gm.StartDateTime.DayOfWeek == DayOfWeek.Saturday && gm.StartDateTime > new DateTime(2015, 3, 1))
                {
                    gm.EndDateTime = gm.StartDateTime.AddHours(3);
                }
                _entities.SaveChanges();
            }
            //List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where (r.StartDateTime.Hour == 0) orderby r.StartDateTime select r).ToList();

            //foreach (GroupMeditation gm in GMs)
            //{
            //    gm.StartDateTime = gm.StartDateTime.AddHours(-1);
            //    _entities.SaveChanges();
            //}

            //DateTime beginDate = new DateTime(2013, 1, 1); // DateTime.Today;
            //DateTime endDate = new DateTime(2013, 12, 31);

            //for (DateTime dt = beginDate; dt <= endDate; dt = dt.AddDays(1))
            //{
            //    GroupMeditation gm = new GroupMeditation();
            //    gm.InitiateTypeID = 1;
            //    gm.StartDateTime = dt.Date;
            //    gm.EndDateTime = gm.StartDateTime.AddHours(7);
            //    if (dt.DayOfWeek == DayOfWeek.Thursday)
            //    {
            //        gm.EndDateTime = gm.StartDateTime.AddHours(6);
            //        if (!_entities.GroupMeditations.Any(a => a.StartDateTime == gm.StartDateTime))
            //        {
            //            _entities.AddToGroupMeditations(gm);
            //            _entities.SaveChanges();
            //        }
            //    }
            //}

            ////for (DateTime dt = beginDate; DateTime.Compare(dt, endDate) <= 0; dt = dt.AddDays(1))
            ////{
            ////    if(dt.DayOfWeek != DayOfWeek.Wednesday && dt.DayOfWeek != DayOfWeek.Friday && !_entities.GroupMeditations.Any(a=>a.StartDateTime == dt))
            ////    {
            ////        GroupMeditation gm = new GroupMeditation();
            ////        gm.InitiateTypeID = 1;
            ////        gm.StartDateTime = dt.Date;
            ////        gm.EndDateTime = gm.StartDateTime.AddHours(7);

            ////        if (dt.DayOfWeek == DayOfWeek.Sunday) 
            ////        {
            ////            if (dt.DayOfWeek == DayOfWeek.Thursday)
            ////            {
            ////                gm.EndDateTime = gm.StartDateTime.AddHours(6);
            ////            }
            ////            _entities.AddToGroupMeditations(gm);
            ////            gm = new GroupMeditation();
            ////            gm.InitiateTypeID = 1;
            ////        }

            ////        if (dt.DayOfWeek == DayOfWeek.Monday || dt.DayOfWeek == DayOfWeek.Tuesday || dt.DayOfWeek == DayOfWeek.Thursday)
            ////        {
            ////            gm.StartDateTime = dt.Date.AddHours(19.5);
            ////            gm.EndDateTime = gm.StartDateTime.AddHours(2);
            ////        }
            ////        else if (dt.DayOfWeek == DayOfWeek.Saturday)
            ////        {
            ////            gm.StartDateTime = dt.Date.AddHours(19);
            ////            gm.EndDateTime = gm.StartDateTime.AddHours(2.5);
            ////        }
            ////        else if (dt.DayOfWeek == DayOfWeek.Sunday)
            ////        {
            ////            gm.StartDateTime = dt.Date.AddHours(9);
            ////            gm.EndDateTime = gm.StartDateTime.AddHours(3);
            ////        }
            ////        else
            ////        {
            ////            continue;
            ////        }

            ////        _entities.AddToGroupMeditations(gm);
            ////        _entities.SaveChanges();
            ////    }

            ////}			
			
																												
			return View();
		}


		[Authorize]
		public ActionResult GMScheduleTable()
        {
            return View();
        }


		[Authorize(Roles = "SuperAdmin")]
        public ActionResult AddNewAttend(int GMID)
		{
            
            var memberRegisterIDs = (from r in _entities.GroupMeditationAttendances where r.GroupMeditationID == GMID select r.MemberID).ToList();
            var memberNotRegisterList = (from r in _entities.MemberInfos 
                                       where !memberRegisterIDs.Contains(r.MemberID) && r.IsActive && !r.Name.StartsWith("0")
                                       orderby r.Name select r).ToList();

            ViewData["MemberInfo"] = memberNotRegisterList;
            ViewData["aEvent"] = GMID;
            ViewData["Descending"] = true;

            return View(memberNotRegisterList.First());
		}
        
        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult AddNewAttend(int GMID, Guid? MemberID, FormCollection collection)
        {
            Guid memberID;
            if (MemberID == null)
            {
                MembershipUser muc = Membership.GetUser(User.Identity.Name);
                memberID = (Guid)muc.ProviderUserKey;
            }
            else
            {
                memberID = (Guid)MemberID;
            }

            //eventRegistration.EventID = eventID;
            GroupMeditation gm = _entities.GroupMeditations.Single(a => a.ID == GMID);
            NewGroupMeditationAttendance(gm.StartDateTime.AddHours(-0.5), memberID, GMID);

            return RedirectToAction("Details", new { eventID = GMID, descending = true });

        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteAttendData(int GMAttendID)
        {
            GroupMeditationAttendance gMAttend = _entities.GroupMeditationAttendances.Single(a => a.ID == GMAttendID);

            return View(gMAttend);
        }

        //
        // POST: /GMVolunteerJobName/Delete/5

        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
        public ActionResult DeleteAttendData(int GMAttendID, FormCollection collection)
        {
            GroupMeditationAttendance gMAttend = _entities.GroupMeditationAttendances.Single(a => a.ID == GMAttendID);
            int GMID = gMAttend.GroupMeditationID;

            _entities.DeleteObject(gMAttend);
            _entities.SaveChanges();

            return RedirectToAction("Details", new { eventID = GMID, descending = true });
        }


		[Authorize(Roles = "SuperAdmin")]
        public ActionResult AddAttendanceData()
		{
			//GetCheckInOutAccessTableData();
            
			//GetAttendanceCsvFile(-5, @"C:\Dropbox\SMCHSGManager\doc\Sep.csv");		// sep 2011
			//GetAttendanceCsvFile(-4, @"C:\Dropbox\SMCHSGManager\doc\Oct.csv");
			GetAttendanceCsvFile(-3, @"C:\Dropbox\SMCHSGManager\doc\Nov.csv");
			GetAttendanceCsvFile(-2, @"C:\Dropbox\SMCHSGManager\doc\Dec.csv");
			GetAttendanceCsvFile(-1, @"C:\Dropbox\SMCHSGManager\doc\Jan.csv");
			//GetAttendanceCsvFile(0, @"C:\Users\raohui\Desktop\SMCH\Feb.csv");		//Feb 2012

            return View();
		}

		public void GetAttendanceCsvFile(int nextMonth, string filePath)
		{
			FileInfo theSourceFile = new FileInfo(filePath);
			StreamReader reader = theSourceFile.OpenText();
			string text;

			List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);
			List<MemberInfo> members = GetMemberInfos();

			int lineNo = 0;
			text = reader.ReadLine();
			while (text != null)
			{
				Guid tMemberID = members[lineNo].MemberID;
				string[] temps = text.Split(',');
				for (int i = 1; i < temps.Length; i++)
				{
					GroupMeditation gm = nextMonthGMs[i - 1];
					if (!string.IsNullOrEmpty(temps[i]) && temps[i].ToLower() == "y" && !_entities.GroupMeditationAttendances.Any(a => a.GroupMeditationID == gm.ID && a.MemberID == tMemberID))
					{
						NewGroupMeditationAttendance(gm.StartDateTime.AddHours(0.5), tMemberID, gm.ID);
					}
				}
				lineNo++;
				text = reader.ReadLine();
			} 

		}

		public void GetCheckInOutAccessTableData()
		{
			//string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Program Files\\att2008\\att2000.mdb"; // +sourceString;
			string conString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source = C:\\SMCH\\SMCHSGManager\\SMCHSGManager\\Data\\att2000.mdb";
			string strSql = "SELECT CHECKINOUT.CHECKTIME, USERINFO.Badgenumber FROM (CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID) ";

			OleDbConnection con = new OleDbConnection(conString);

			DataTable AttendanceTable = new DataTable();

			con.Open();
			OleDbDataAdapter dAdapter = new OleDbDataAdapter();
			dAdapter.SelectCommand = new OleDbCommand(strSql, con);
			dAdapter.Fill(AttendanceTable);
			con.Close();

		

			foreach (DataRow dr in AttendanceTable.Rows)
			{
				DateTime checkInTime = (DateTime)dr["CHECKTIME"];

				int memberNo = int.Parse((string)dr["Badgenumber"]);

				if (!_entities.MemberInfos.Any(a => a.MemberNo == memberNo) || checkInTime < new DateTime(2011, 12, 1))
				{
					continue;
				}

				Guid memberID = _entities.MemberInfos.SingleOrDefault(a => a.MemberNo == memberNo).MemberID;

				DateTime checkInTimeStart = checkInTime.AddHours(1.5);
				if (_entities.Events.Any(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime))
				{
					int eventID = _entities.Events.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime).FirstOrDefault().ID;

					if (!_entities.EventRegistrations.Any(a => a.EventID == eventID && a.MemberID == memberID))
					{
						EventRegistration eventRegistration = new EventRegistration();
						eventRegistration.MemberID = memberID;
						eventRegistration.EventID = eventID;
						eventRegistration.SignTime = checkInTime;
						_entities.AddToEventRegistrations(eventRegistration);
						_entities.SaveChanges();
						continue;
					}
				}

				if (_entities.GroupMeditations.Any(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime))
				{
					int groupMeditaionID = _entities.GroupMeditations.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime).FirstOrDefault().ID;

					if (!_entities.GroupMeditationAttendances.Any(a=>a.GroupMeditationID == groupMeditaionID && a.MemberID == memberID))
					{
						NewGroupMeditationAttendance(checkInTime, memberID, groupMeditaionID);
					}
				}
			}

		}

		private void NewGroupMeditationAttendance(DateTime checkInTime, Guid memberID, int groupMeditaionID)
		{
			GroupMeditationAttendance groupMeditationAttendance = new GroupMeditationAttendance();
			groupMeditationAttendance.MemberID = memberID;
			groupMeditationAttendance.GroupMeditationID = groupMeditaionID;
			groupMeditationAttendance.CheckInTime = checkInTime;
			_entities.AddToGroupMeditationAttendances(groupMeditationAttendance);
			_entities.SaveChanges();
		}


		[Authorize]
		public ActionResult MyGroupMeditation(Guid? memberID, DateTime? startDate, DateTime? endDate)
		{

			if (memberID == null)
			{
				MembershipUser user = Membership.GetUser();
				memberID = ((Guid)(user.ProviderUserKey));
			}

			var gmAttendances = (from r in _entities.GroupMeditationAttendances
								  where r.MemberInfo.MemberID == memberID &&
											  (startDate == null || r.GroupMeditation.StartDateTime >= startDate.Value) &&
												(endDate == null || r.GroupMeditation.EndDateTime <= endDate.Value)
								  orderby r.MemberInfo.Name, r.CheckInTime
								  select r).ToList();
	
			return View(gmAttendances);
		}

		[Authorize(Roles = "Administrator")]
	    public ActionResult GMPast6MonthsHistory(int eventTypeID, string searchContent)
	    {

			ViewData["searchContent"] = searchContent;

			int memberNO = 0;
			EventSignatureController erc = new EventSignatureController();
			if (!string.IsNullOrEmpty(searchContent) && erc.IsInteger(searchContent))
			{
				memberNO = int.Parse(searchContent);
			}

			var querys = (from r in _entities.GroupMeditationAttendances
					  where (r.MemberInfo.Name.Contains(searchContent) ||
									searchContent == null || r.MemberInfo.MemberNo == memberNO)
									 && r.MemberInfo.MemberNo.HasValue && r.MemberInfo.MemberNo.Value < 1000
									 && !r.MemberInfo.Name.Contains("tester")
					  orderby r.MemberInfo.MemberNo

					  group r by new
					  {
						  memberInfo = r.MemberInfo,
					  } into result
					  select new GMAttendanceCountViewModel()
					  {
						  MemberInfo = result.Key.memberInfo,
						   TotalCount = result.Count(),
					  }).OrderBy(a=>a.MemberInfo.MemberNo).ToList();

			InternationalGMApplicationController igmac = new InternationalGMApplicationController ();
			
		    List<GMAttendanceCountViewModel> viewModel = new List<GMAttendanceCountViewModel>();

		    foreach (GMAttendanceCountViewModel query in querys)
		    {
			    GMAttendanceCountViewModel gmAttendanceCountViewModel = igmac.GetGMAttendanceMonthlyCounts(6, query.MemberInfo);
                viewModel.Add(gmAttendanceCountViewModel);
		    }

		    return View(viewModel);
        }



		[Authorize(Roles = "Administrator")]
		public ActionResult AttendanceTable(int nextMonth)
		{
			List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);

			DateTime firstDayOfMonth = new DateTime(DateTime.Today.AddMonths(nextMonth).Year, DateTime.Today.AddMonths(nextMonth).Month, 1);

			DateTime nextMonthDay = firstDayOfMonth.AddDays(32);
			DateTime preMonthDay = firstDayOfMonth.AddDays(-2);

			List<MemberInfo> members = GetMemberInfos();
			updateMemberFeeExipredDate(members);

			var viewModel = new GMAttendanceViewModel
			{
				Members = members,
				nextMonthGMs = nextMonthGMs,
				NextMonth = nextMonth,
				NextMonthStr = (string.Format("{0: MMMM, yyyy}", firstDayOfMonth)).Trim(),
				HaveNextMonth = _entities.GroupMeditations.Any(a => a.StartDateTime >= nextMonthDay),
				HavePreviousMonth = _entities.GroupMeditations.Any(a => a.StartDateTime <= preMonthDay),
			};

			bool[,] attendenceChecks = new bool[members.Count, nextMonthGMs.Count];
			int j = 0;
			foreach (GroupMeditation gm in nextMonthGMs)
			{
				List<MemberInfo> mis = _entities.GroupMeditationAttendances.Where(a => a.GroupMeditationID == gm.ID).Select(a => a.MemberInfo).OrderBy(a => a.MemberNo).ToList();
				for (int i = 0; i < members.Count; i++)
				{
					if (mis.Any(a => a.MemberNo == members[i].MemberNo))
					{
						attendenceChecks[i, j] = true;
					}
				}
				j++;
			}

			viewModel.AttendenceChecks = attendenceChecks;

			TempData["viewModel"] = viewModel;

			return View(viewModel);
		}

		private List<MemberInfo> GetMemberInfos()
		{
            List<MemberInfo> members = _entities.MemberInfos.Where(a => a.MemberNo.HasValue && a.MemberNo < 999 && a.Name != "0" && a.Name != "DP" && a.IsActive).OrderBy(a => a.MemberNo).ToList();
			return members;
		}

		public void updateMemberFeeExipredDate(List<MemberInfo> memberInfos)
		{

			var latestMemberFeePayments = (from r in _entities.MemberFeePayments
										   orderby r.ToDate descending
										   group r by r.MemberInfo.MemberNo into h
										   select new
										   {
											   MemberNo = h.Key,
											   ToDate = h.Max(a => a.ToDate),
										   }).ToList();

			foreach (var mfp in latestMemberFeePayments)
			{
				if (memberInfos.Any(a => a.MemberNo == mfp.MemberNo))
				{
					MemberInfo memberInfo = memberInfos.SingleOrDefault(a => a.MemberNo == mfp.MemberNo);
                    if (mfp.ToDate > memberInfo.MemberFeeExpiredDate || !memberInfo.MemberFeeExpiredDate.HasValue)
					{
                        memberInfo.MemberFeeExpiredDate = mfp.ToDate;
                        UpdateModel(memberInfo, "MemberInfo");
                        _entities.SaveChanges();
					}
				}
			}


		}


		[HttpPost]
		public ActionResult AttendanceTable(int nextMonth, FormCollection collection)
		{

			List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);

			List<MemberInfo> members = GetMemberInfos();

			try
			{
				string[] attendenceChecks = (string[])collection.GetValues("AttendenceChecks");

				int i = 0;
				foreach(MemberInfo mi in members)
				{
					foreach (GroupMeditation gm in nextMonthGMs)
					{
						if (bool.Parse(attendenceChecks[i++]))
						{
							GroupMeditationAttendance groupMeditationAttendance = new GroupMeditationAttendance();
							groupMeditationAttendance.MemberID = mi.MemberID;
							groupMeditationAttendance.GroupMeditationID = gm.ID;
							groupMeditationAttendance.CheckInTime = gm.StartDateTime.AddHours(0.5);
							_entities.AddToGroupMeditationAttendances(groupMeditationAttendance);
							_entities.SaveChanges();
						}
					}
				}

				return RedirectToAction("index");
			}
			catch
			{
				return View();
			}
		}

		private static List<GroupMeditation> GetMonthGMs(int nextMonth)
		{
			GMVolunteerJobNameController gmvj = new GMVolunteerJobNameController();
			List<GroupMeditation> nextMonthGMs = gmvj.GetMonthGMs(nextMonth);
			nextMonthGMs = nextMonthGMs.Where(a => a.InitiateTypeID == 1).OrderBy(a => a.StartDateTime).ToList();
			return nextMonthGMs;
		}

		public string[][] GetStringList()
		{
			var viewModel = (GMAttendanceViewModel)TempData["viewModel"];
			string[][] tempList = new string[3][]
			{
				new string[] {"MemberNo", "Name", "MemberFeeExpiredDate", viewModel.NextMonthStr},
				new string[] {},
				new string[] {},
			};

			tempList[1] = new string[viewModel.nextMonthGMs.Count];
			int i = 0;
			foreach (GroupMeditation gm in viewModel.nextMonthGMs)
			{
				string week = gm.StartDateTime.DayOfWeek.ToString().Substring(0, 3);
				if (gm.StartDateTime.Hour == 0)
				{
					week = gm.StartDateTime.DayOfWeek.ToString().Substring(0, 1)+ "-O"; 
				}
				tempList[1][i++] = week;
			}

			i = 0;
			tempList[2] = new string[viewModel.nextMonthGMs.Count];
			foreach (GroupMeditation gm in viewModel.nextMonthGMs)
			{
				tempList[2][i++] = gm.StartDateTime.Day.ToString();
			}

			return tempList;
		}

		public ActionResult GenerateExcel2()
		{
			var viewModel = (GMAttendanceViewModel)TempData["viewModel"];
			return this.Excel(null, viewModel.Members.AsQueryable(), "AttendanceTable.xls", GetStringList());
		}

	     
    }
}
