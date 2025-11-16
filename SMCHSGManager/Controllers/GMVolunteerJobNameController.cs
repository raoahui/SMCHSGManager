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
    public class GMVolunteerJobNameController : Controller
    {

		private SMCHDBEntities _entities = new SMCHDBEntities();

        //
        // GET: /GMVolunteerJobName/

		[Authorize(Roles = "Initiate")]
		public ActionResult Index(int? volunteerJobTypeID)
        {
			var viewModel = _entities.GMVolunteerJobNames.OrderBy(a=>a.MemberInfo.Name).ToList();
			if (volunteerJobTypeID.HasValue)
			{
				viewModel = _entities.GMVolunteerJobNames.Where(a => a.VolunteerJobTypeID == volunteerJobTypeID.Value).OrderBy(a => a.MemberInfo.Name).ToList();
				ViewData["volunteerJobTypeID"] = volunteerJobTypeID;
			}
			
			return View(viewModel);
        }

        //
        // GET: /GMVolunteerJobName/Details/5

		//public ActionResult Details(int id)
		//{
		//    return View();
		//}

        //
        // GET: /GMVolunteerJobName/Create
		[Authorize(Roles = "Administrator")]
		public ActionResult Create(int volunteerJobTypeID)
        {
			var gmVolunteerJobNames = (from r in _entities.GMVolunteerJobNames where r.VolunteerJobTypeID == volunteerJobTypeID select r.MemberID).ToList();
			var memberNotVJID = (from r in _entities.MemberInfos where !gmVolunteerJobNames.Contains(r.MemberID) orderby r.Name select r).ToList();
			var viewModel = new GMVolunteerJobNameViewdModel
			{
				 GMVolunteerJobName = new GMVolunteerJobName (),
				 MemberInfo = memberNotVJID,
			};
			viewModel.GMVolunteerJobName.VolunteerJobTypeID = volunteerJobTypeID;
			ViewData["VolunteerJobTypeName"] = _entities.VolunteerJobTypes.Single(a => a.ID == volunteerJobTypeID).Name;

		return View(viewModel);
        } 

        //
        // POST: /GMVolunteerJobName/Create

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(int volunteerJobTypeID, FormCollection collection, GMVolunteerJobName gmVolunteerJobName)
        {
            try
            {
				gmVolunteerJobName.VolunteerJobTypeID = volunteerJobTypeID;
				_entities.AddToGMVolunteerJobNames(gmVolunteerJobName);
				_entities.SaveChanges();

				return RedirectToAction("Index", new { volunteerJobTypeID = volunteerJobTypeID });
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /GMVolunteerJobName/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(Guid memberID, int volunteerJobTypeID)
        {
			var gmVolunteerJobNames = (from r in _entities.GMVolunteerJobNames where r.VolunteerJobTypeID == volunteerJobTypeID select r.MemberID).ToList();
			var memberNotVJID = (from r in _entities.MemberInfos where !gmVolunteerJobNames.Contains(r.MemberID) orderby r.Name select r).ToList();

			GMVolunteerJobName gmVolunteerJobName = _entities.GMVolunteerJobNames.Single(a => a.MemberID == memberID && a.VolunteerJobTypeID == volunteerJobTypeID);
            var viewModel = new GMVolunteerJobNameViewdModel
			{
				GMVolunteerJobName = gmVolunteerJobName,
				MemberInfo = memberNotVJID,
			};
			ViewData["VolunteerJobTypeName"] = gmVolunteerJobName.VolunteerJobType.Name;

			return View(viewModel);
        }

        //
        // POST: /GMVolunteerJobName/Edit/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(Guid memberID, int volunteerJobTypeID, FormCollection collection)
        {
			GMVolunteerJobName gmVolunteerJobName = _entities.GMVolunteerJobNames.Single(a => a.MemberID == memberID && a.VolunteerJobTypeID == volunteerJobTypeID);
            try
            {
				UpdateModel(gmVolunteerJobName, "GMVolunteerJobName");
				_entities.SaveChanges();
				return RedirectToAction("Index", new { volunteerJobTypeID = volunteerJobTypeID });
            }
            catch
            {
                return View();
            }
        }

		//
		// GET: /GMVolunteerJobName/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(Guid memberID, int volunteerJobTypeID)
		{
			GMVolunteerJobName gmVolunteerJobName = _entities.GMVolunteerJobNames.Single(a => a.MemberID == memberID && a.VolunteerJobTypeID == volunteerJobTypeID);
			return View(gmVolunteerJobName);
		}

		//
		// POST: /GMVolunteerJobName/Delete/5

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(Guid memberID, int volunteerJobTypeID, FormCollection collection)
		{
			GMVolunteerJobName gmVolunteerJobName = _entities.GMVolunteerJobNames.Single(a => a.MemberID == memberID && a.VolunteerJobTypeID == volunteerJobTypeID);

			ViewData["Name"] = gmVolunteerJobName.MemberInfo.Name;
			ViewData["volunteerJobTypeID"] = gmVolunteerJobName.VolunteerJobTypeID;
			_entities.DeleteObject(gmVolunteerJobName);
			_entities.SaveChanges();

			return View("Deleted");
		}


		[Authorize]
		public ActionResult DPRoster(int nextMonth, bool edit)
		{
            int numberOfSundays = NumberOfParticularDaysInMonth(2010, 9, DayOfWeek.Sunday);
            int maxRow = numberOfSundays+1;

            List<Tuple<DayOfWeek, int, string>> titleList = new List<Tuple<DayOfWeek, int, string>>();
            List<List<MemberInfo>> weekNoDPLists = GetWeekNoDPLists(nextMonth, ref maxRow, ref titleList);
            
            string[,] monthDpList = new string[maxRow, weekNoDPLists.Count];
            DateTime firstDayOfMonth = GetDpMonthMatrics(monthDpList, titleList, nextMonth, edit);

            List<string> tWeekNameList = new List<string>();
            List<string> tTimeList = new List<string>();
            SeparateTitles(titleList, tWeekNameList, tTimeList);
			DateTime nextMonthDay = firstDayOfMonth.AddDays(32);
			DateTime preMonthDay = firstDayOfMonth.AddDays(-2);
	
			var viewModel = new DPRosterViewModel
			{
                TitleWeekNameList = tWeekNameList,
                TitleTimeList = tTimeList,
				WeekNoDPLists = weekNoDPLists,
				MonthDpList = monthDpList,
				NextMonth = nextMonth,
				NextMonthStr = (string.Format("{0:y}", firstDayOfMonth)).Trim('^'),
				Edit = edit,
				HaveNextMonth = _entities.GroupMeditations.Any(a => a.StartDateTime >= nextMonthDay),
				HavePreviousMonth = _entities.GroupMeditations.Any(a => a.StartDateTime <= preMonthDay),
			};

			return View(viewModel);
		}

        private static void SeparateTitles(List<Tuple<DayOfWeek, int, string>> titleList, List<string> tWeekNameList, List<string> tTimeList)
        {
            if (titleList.Count > 0)
            {
                int repeat = 1;
                string pWeekName = "";
                foreach (var item in titleList)
                {
                    if (pWeekName == item.Item1.ToString())
                    {
                        repeat++;
                        tWeekNameList.RemoveAt(tWeekNameList.Count - 1);
                    }
                    else repeat = 1;
                    string weekNo = item.Item1.ToString() + "^" + repeat.ToString();
                    tWeekNameList.Add(weekNo);
                    tTimeList.Add(item.Item3);
                    pWeekName = item.Item1.ToString();
                }
            }
        }

        private DateTime GetDpMonthMatrics(string[,] monthDpList, List<Tuple<DayOfWeek, int, string>> titleList, int nextMonth, bool edit)
        {
            List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.AddMonths(nextMonth).Year, DateTime.Today.AddMonths(nextMonth).Month, 1);
            if (nextMonthGMs.Count == 0) return firstDayOfMonth;

            int row = 0;
            MemberInfo mi = _entities.MemberInfos.Single(a => a.Name == "DP");
            foreach (GroupMeditation gm in nextMonthGMs)
            {
                var curWeekNo = getCurWeekNo(gm);
                int column = titleList.BinarySearch(curWeekNo);
               
                monthDpList[row, column] = gm.StartDateTime.Day.ToString();
                if (gm.DPMemberID.HasValue)
                {
                    if (edit)   monthDpList[row, column] += '^' + gm.DPMemberID.ToString();
                    else
                    {
                        MemberInfo me = _entities.MemberInfos.Single(a => a.MemberID == gm.DPMemberID);
                        monthDpList[row, column] += '^' + me.Name;
                        if (!string.IsNullOrEmpty(me.ContactNo) && (Roles.IsUserInRole("SuperAdmin") || Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("DP Admin")))
                             monthDpList[row, column] += " (" + me.ContactNo + ')';
                     }
                }
                else
                {
                    monthDpList[row, column] += '^';
                    if (edit)  monthDpList[row, column] += mi.MemberID.ToString();
                }
                if (column == titleList.Count - 1) row++;
            }

            return firstDayOfMonth;
        }

        public List<GroupMeditation>  GetMonthGMs(int nextMonth)
		{
			int nextMonthNo = DateTime.Today.AddMonths(nextMonth).Month;
			int nextMonthYear = DateTime.Today.AddMonths(nextMonth).Year;
			DateTime firstDayOfMonth = new DateTime(nextMonthYear, nextMonthNo, 1);
			if (nextMonthNo == 12)
			{
				nextMonthNo = 0;
				nextMonthYear++;
			}
			DateTime lastDayOfMonth = (new DateTime(nextMonthYear, nextMonthNo + 1, 1)).AddMinutes(-1);

			List<GroupMeditation> nextMonthGMs = _entities.GroupMeditations.
                                    Where(a => a.StartDateTime >= firstDayOfMonth && a.StartDateTime <= lastDayOfMonth 
                                        && (a.InitiateTypeID == 1 || a.InitiateTypeID == 3)).OrderBy(a => a.StartDateTime).ToList();

			return nextMonthGMs;
		}

        static int NumberOfParticularDaysInMonth(int year, int month, DayOfWeek dayOfWeek)
        {
            DateTime startDate = new DateTime(year, month, 1);
            int totalDays = startDate.AddMonths(1).Subtract(startDate).Days;

            int answer = Enumerable.Range(1, totalDays)
                .Select(item => new DateTime(year, month, item))
                .Where(date => date.DayOfWeek == dayOfWeek)
                .Count();

            return answer;
        }

        private List<List<MemberInfo>> GetWeekNoDPLists(int nextMonth, ref int maxRow, ref List<Tuple<DayOfWeek, int, string>> titleList)
		{
            List<List<MemberInfo>> weekNoDpList = new List<List<MemberInfo>>();
            List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);
            if (nextMonthGMs.Count == 0) return weekNoDpList;

            int curYear = nextMonthGMs.First().StartDateTime.Year;
            int curMonth = nextMonthGMs.First().StartDateTime.Month;
            int numberOfSundays = NumberOfParticularDaysInMonth(curYear, curMonth, DayOfWeek.Sunday);
            maxRow = numberOfSundays;
            if (nextMonthGMs.First().StartDateTime.DayOfWeek != DayOfWeek.Sunday) maxRow++;

            MemberInfo dpMemberInfo = _entities.MemberInfos.Single(a => a.Name == "DP");
            var weekNoList = new List<Tuple<DayOfWeek, int, string>>();
            foreach (GroupMeditation gm in nextMonthGMs)
			{
                var curWeekNo = getCurWeekNo(gm);
                if (!weekNoList.Contains(curWeekNo))
                {
                    weekNoList.Add(curWeekNo);
                }
            }

            // generate Sunday on the first
            titleList = weekNoList.OrderBy(a => a.Item1).ThenBy(a => a.Item2).ToList();
            foreach (var item in titleList)
            {
                List<MemberInfo> weekNoMemberInfo = (from r in _entities.GMVolunteerJobNames
                                               join h in _entities.MemberInfos on r.MemberID equals h.MemberID
                                               orderby h.Name
                                               select h).ToList();
                weekNoMemberInfo.Insert(0, dpMemberInfo);
                    //GetWeekNoDpMap(mi, item.Item1, item.Item2);
                weekNoDpList.Add(weekNoMemberInfo);
            }
            return weekNoDpList;
		}

        private static Tuple<DayOfWeek, int, string> getCurWeekNo(GroupMeditation gm)
        {
            var dateTimeDPStart = gm.StartDateTime.AddMinutes(-30);
            string weekNoName = String.Format("{0:t}", dateTimeDPStart) + " - " + String.Format("{0:t}", gm.StartDateTime);
            var curWeekNo = new Tuple<DayOfWeek, int, string>(dateTimeDPStart.DayOfWeek, dateTimeDPStart.Hour, weekNoName);
            return curWeekNo;
        }

        //private List<MemberInfo> GetWeekNoDpMap(MemberInfo mi, DayOfWeek dow, int startTime)
        //{
        //    List<MemberInfo> tempList = new List<MemberInfo>();
        //    switch (dow)
        //    {
        //        case DayOfWeek.Sunday:
        //            {
        //                if (startTime > 8 && startTime < 10)
        //                    tempList = (from r in _entities.GMVolunteerJobNames
        //                                join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                                where r.Sunday && r.VolunteerJobTypeID == 1
        //                                orderby h.Name
        //                                select h).ToList();
        //                else if (startTime > 18 && startTime < 20)
        //                    tempList = (from r in _entities.GMVolunteerJobNames
        //                                join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                                where r.SundayEvening && r.VolunteerJobTypeID == 1
        //                                orderby h.Name
        //                                select h).ToList();
        //            }
        //            break;
        //        case DayOfWeek.Monday:
        //            tempList = (from r in _entities.GMVolunteerJobNames
        //                        join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                        where r.Monday && r.VolunteerJobTypeID == 1
        //                        orderby h.Name
        //                        select h).ToList();
        //            break;
        //        case DayOfWeek.Tuesday:
        //            tempList = (from r in _entities.GMVolunteerJobNames
        //                        join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                        where r.Tuesday && r.VolunteerJobTypeID == 1
        //                        orderby h.Name
        //                        select h).ToList();
        //            break;
        //        case DayOfWeek.Wednesday:
        //            if (startTime > 18 && startTime < 20)
        //                tempList = (from r in _entities.GMVolunteerJobNames
        //                            join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                            where r.Wednesday && r.VolunteerJobTypeID == 1
        //                            orderby h.Name
        //                            select h).ToList();
        //            else if (startTime > 22)
        //                tempList = (from r in _entities.GMVolunteerJobNames
        //                            join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                            where r.WednesdayOvernight && r.VolunteerJobTypeID == 1
        //                            orderby h.Name
        //                            select h).ToList();
        //            break;
        //        case DayOfWeek.Thursday:
        //            tempList = (from r in _entities.GMVolunteerJobNames
        //                        join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                        where r.Thursday && r.VolunteerJobTypeID == 1
        //                        orderby h.Name
        //                        select h).ToList();
        //            break;
        //        case DayOfWeek.Friday:
        //            tempList = (from r in _entities.GMVolunteerJobNames
        //                        join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                        where r.Friday && r.VolunteerJobTypeID == 1
        //                        orderby h.Name
        //                        select h).ToList();
        //            break;
        //        case DayOfWeek.Saturday:
        //            {
        //                if (startTime > 8 && startTime < 10)
        //                    tempList = (from r in _entities.GMVolunteerJobNames
        //                                join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                                where r.SaturdayDay && r.VolunteerJobTypeID == 1
        //                                orderby h.Name
        //                                select h).ToList();
        //                else if (startTime > 18 && startTime < 20)
        //                    tempList = (from r in _entities.GMVolunteerJobNames
        //                                join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                                where r.SaturdayEvening && r.VolunteerJobTypeID == 1
        //                                orderby h.Name
        //                                select h).ToList();
        //                else if (startTime > 22)
        //                    tempList = (from r in _entities.GMVolunteerJobNames
        //                                join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                                where r.SaturdayOvernight && r.VolunteerJobTypeID == 1
        //                                orderby h.Name
        //                                select h).ToList();
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //    if (tempList.Count == 0) tempList = (from r in _entities.GMVolunteerJobNames
        //                                         join h in _entities.MemberInfos on r.MemberID equals h.MemberID
        //                                         orderby h.Name
        //                                         select h).ToList();
        //    tempList.Insert(0, mi);
        //    return tempList;
        //}

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult DPRoster(int nextMonth, FormCollection collection, GMVolunteerJobName gmVolunteerJobName)
		{
			try
			{
				string[] iMemberIDs = ((string)collection.Get("IMemberID")).Split(',');
				List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);

				int i = 0;
				foreach(GroupMeditation gm in nextMonthGMs)
				{
                        gm.DPMemberID = Guid.Parse(iMemberIDs[i]);
                        _entities.SaveChanges();
                        i++;
				}
				return RedirectToAction("DPRoster", new { nextMonth = nextMonth, edit = false });
			}
			catch
			{
				return View();
			}
		}

    }
}
