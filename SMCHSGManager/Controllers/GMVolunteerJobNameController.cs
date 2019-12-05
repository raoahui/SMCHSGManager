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

			//ViewData["MemberInfo"] = viewModel.MemberInfo ;
		
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
			//ViewData["MemberInfo"] = viewModel.MemberInfo;

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
            List<List<MemberInfo>> weekNoDPLists = GetWeekNoDPLists(nextMonth);

            string[,] monthDpList = new string[6, weekNoDPLists.Count];
			DateTime firstDayOfMonth = GetDpMonthMatrics(monthDpList, weekNoDPLists, nextMonth, edit);

			DateTime nextMonthDay = firstDayOfMonth.AddDays(32);
			DateTime preMonthDay = firstDayOfMonth.AddDays(-2);
	
			var viewModel = new DPRosterViewModel
			{
				WeekNoDPLists = weekNoDPLists,
				MonthDpList = monthDpList,
				NextMonth = nextMonth,
				NextMonthStr = (string.Format("{0:y}", firstDayOfMonth)).Trim('^'),
				Edit = edit,
				HaveNextMonth = _entities.GroupMeditations.Any(a => a.StartDateTime >= nextMonthDay),
				HavePreviousMonth = _entities.GroupMeditations.Any(a => a.StartDateTime <= preMonthDay),
			};

            ViewData["FirstDayOfMonth"] = firstDayOfMonth;
			return View(viewModel);
		}

		private DateTime GetDpMonthMatrics(string[,] monthDpList, List<List<MemberInfo>> memberLists, int nextMonth, bool edit)
		{

			List<GroupMeditation> nextMonthGMs = GetMonthGMs(nextMonth);
			DateTime firstDayOfMonth = new DateTime(DateTime.Today.AddMonths(nextMonth).Year, DateTime.Today.AddMonths(nextMonth).Month, 1);

			int i = 0; 
			int j = -1; // indicate the first of line.
			foreach (GroupMeditation gm in nextMonthGMs)
			{
                if (!isGMInRosterList(gm))
                {
                    continue;
                }

                if (j < 0)
                {
                    j = gm.StartDateTime.DayOfWeek - DayOfWeek.Sunday;
                }
                else
                {
                    j++;
                }

				i = GetMonthDPContents(monthDpList, gm, i, j, memberLists[i][0].MemberID, edit);

                if (j == memberLists.Count - 1 && i < monthDpList.GetLength(0))
                {
                    i++;
                    j = -1;
                }

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

			//return firstDayOfMonth;
			return nextMonthGMs;
		}


        private List<List<MemberInfo>> GetWeekNoDPLists(int nextMonth)
		{
			List<List<MemberInfo>> WeekNoDPLists = new List<List<MemberInfo>>();

            MemberInfo mi = _entities.MemberInfos.Single(a => a.Name == "DP");
			List<MemberInfo> tempList = (from r in _entities.GMVolunteerJobNames
									 join h in _entities.MemberInfos on r.MemberID equals h.MemberID
									 where r.Sunday && r.VolunteerJobTypeID == 1
									 orderby h.Name
									 select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

			tempList = (from r in _entities.GMVolunteerJobNames
						join h in _entities.MemberInfos on r.MemberID equals h.MemberID
						where r.Monday && r.VolunteerJobTypeID == 1
						orderby h.Name
						select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

			tempList = (from r in _entities.GMVolunteerJobNames
						join h in _entities.MemberInfos on r.MemberID equals h.MemberID
						where r.Tuesday && r.VolunteerJobTypeID == 1
						orderby h.Name
						select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

            DateTime firstDayOfMonth = new DateTime(DateTime.Today.AddMonths(nextMonth).Year, DateTime.Today.AddMonths(nextMonth).Month, 1);
            if (firstDayOfMonth >= new DateTime(2012, 5, 1))
            {
                tempList = (from r in _entities.GMVolunteerJobNames
                            join h in _entities.MemberInfos on r.MemberID equals h.MemberID
                            where r.WednesdayOvernight && r.VolunteerJobTypeID == 1
                            orderby h.Name
                            select h).ToList();
                tempList.Insert(0, mi);
                WeekNoDPLists.Add(tempList);
            }

			tempList = (from r in _entities.GMVolunteerJobNames
						join h in _entities.MemberInfos on r.MemberID equals h.MemberID
						where r.Thursday && r.VolunteerJobTypeID == 1
						orderby h.Name
						select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

			tempList = (from r in _entities.GMVolunteerJobNames
						join h in _entities.MemberInfos on r.MemberID equals h.MemberID
						where r.Friday && r.VolunteerJobTypeID == 1
						orderby h.Name
						select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

            if (firstDayOfMonth >= new DateTime(2015, 3, 1))
            {
                tempList = (from r in _entities.GMVolunteerJobNames
                            join h in _entities.MemberInfos on r.MemberID equals h.MemberID
                            where r.SaturdayDay && r.VolunteerJobTypeID == 1
                            orderby h.Name
                            select h).ToList();
                tempList.Insert(0, mi);
                WeekNoDPLists.Add(tempList);
            }

            tempList = (from r in _entities.GMVolunteerJobNames
						join h in _entities.MemberInfos on r.MemberID equals h.MemberID
						where r.SaturdayEvening && r.VolunteerJobTypeID == 1
						orderby h.Name
						select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

			tempList = (from r in _entities.GMVolunteerJobNames
						join h in _entities.MemberInfos on r.MemberID equals h.MemberID
						where r.SaturdayOvernight && r.VolunteerJobTypeID == 1
						orderby h.Name
						select h).ToList();
            tempList.Insert(0, mi);
            WeekNoDPLists.Add(tempList);

			return WeekNoDPLists;
		}


		private int GetMonthDPContents(string[,] monthDpList, GroupMeditation gm, int i, int weekNO, Guid  memberID, bool edit)
		{
            int new_i = i;
            if (string.IsNullOrEmpty(monthDpList[new_i, weekNO]))
            {
                monthDpList[new_i, weekNO] = gm.StartDateTime.Day.ToString();
            }
            else
            {
                new_i++;
            }
            monthDpList[new_i, weekNO] += '^' + String.Format("{0:t}", gm.StartDateTime) + " - " + String.Format("{0:t}", gm.EndDateTime);
			if (gm.DPMemberID.HasValue)
			{
				if (edit)
				{
                    monthDpList[new_i, weekNO] += '^' + gm.DPMemberID.ToString();
				}
				else
				{
                    MemberInfo mi = _entities.MemberInfos.Single(a => a.MemberID == gm.DPMemberID);
                    monthDpList[new_i, weekNO] += '^' + mi.Name;
                    if (!string.IsNullOrEmpty(mi.ContactNo) && (Roles.IsUserInRole("SuperAdmin") || Roles.IsUserInRole("DP Admin")))
                    {
                        monthDpList[new_i, weekNO] += " (" + mi.ContactNo + ')';
                    }
				}
			}
			else
			{
				if (edit)
				{
                    monthDpList[new_i, weekNO] += '^' + memberID.ToString();
				}
				else
				{
                    monthDpList[new_i, weekNO] += '^';
				}
			}

            return new_i;
		}

	
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
                    if (isGMInRosterList(gm))
                    {
                        gm.DPMemberID = Guid.Parse(iMemberIDs[i]);
                        _entities.SaveChanges();
                        i++;
                    }
				}

				return RedirectToAction("DPRoster", new { nextMonth = nextMonth, edit = false });
			}
			catch
			{
				return View();
			}
		}


        private static bool isGMInRosterList(GroupMeditation gm)
        {
            bool retVal = false;

            if ( gm.StartDateTime.DayOfWeek == DayOfWeek.Sunday && gm.StartDateTime.Hour == 9 || 
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Monday && gm.StartDateTime.Hour   == 19 ||
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Tuesday && gm.StartDateTime.Hour  == 19 ||
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Thursday && gm.StartDateTime.Hour == 19 ||
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Friday && gm.StartDateTime.Hour == 20 && gm.InitiateTypeID == 3 ||
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Saturday && gm.StartDateTime.Hour == 19 ||
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Wednesday && gm.StartDateTime.Hour == 23 && gm.StartDateTime >= new DateTime(2012, 5, 1) || // Wednesday overnight session 
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Saturday && gm.StartDateTime.Hour == 23 ||    // Saturday overnight session
                 gm.StartDateTime.DayOfWeek == DayOfWeek.Saturday && gm.StartDateTime.Hour == 9 && gm.StartDateTime >= new DateTime(2015, 3, 1) // Saturday morning session
               )
            
            {
                retVal = true;
            }

            return retVal;
        }





    }
}
