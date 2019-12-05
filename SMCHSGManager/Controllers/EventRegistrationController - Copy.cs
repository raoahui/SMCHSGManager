using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Globalization;
using System.Web.Security;
using System.Text;

namespace SMCHSGManager.Controllers
{
    public class EventRegistrationController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();
        private readonly string[] _registerHeads = new string[] { "S/N", "Name in ID Card", "ID Card No", "Contact No", "Volunteer Job", " Meals Selection", "To Pay", "Remark" };

        //
        // GET: /EventRegister/

        public ActionResult IndexByEvent(int eventID)
        {
            var eventRegistrations = (from r in _entities.EventRegistrations
                                     where (r.EventID == eventID)
                                     orderby r.MemberInfo.Name
                                     select r).ToList();

            return View(eventRegistrations);
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int eventID)
        {
            ViewData["LocalRetreatID"] = eventID;
            Event aEvent = _entities.Events.Single(a => a.ID == eventID);
            int totalGuests = 0;

            List<EventRegistration> eventRegistrations = (from r in _entities.EventRegistrations where r.EventID == eventID orderby r.MemberInfo.Name select r).ToList();

            if (aEvent.EventTypeID == 5)
            {
                eventRegistrations = eventRegistrations.OrderBy(a => a.BackDateTime).ThenBy(a => a.MemberInfo.MemberNo).ToList();
            }

            List<List<string>> eventRegistrationList = new List<List<string>>();

            List<string> mLabels;
            List<string> mValues;
            GetMealNameDate(eventID, out mLabels, out mValues, true);

            decimal totalPrice = 0;
            int iRow = 1;
            List<string> row = new List<string>();
            int[] mealCounts = new int[mValues.Count];
            foreach (var eventRegistration in eventRegistrations)
            {
                row.Add(eventRegistration.ID.ToString());
                row.Add(eventRegistration.MemberInfo.Name);
                row.Add(eventRegistration.MemberInfo.MemberNo.ToString());
                row.Add(eventRegistration.MemberInfo.IDCardNo);
                row.Add(eventRegistration.MemberInfo.ContactNo);

                List<string> volunteerJobLabels = new List<string>();
                List<string> volunteerJobValues = new List<string>();
                GetEventVolunteerJobBooking(eventRegistration.ID, volunteerJobLabels, volunteerJobValues, "name");

                int j = 0;
                string volunteerJobStr = string.Empty;
                foreach (string s in volunteerJobLabels)
                {
                    volunteerJobStr += s + ": " + volunteerJobValues.ElementAt(j++);
                    if (j < volunteerJobLabels.Count())
                    {
                        volunteerJobStr += "\n";
                    }
                }
                row.Add(volunteerJobStr);

                List<int> mealBookingScheduleIDs = (from r in _entities.EventMealBookings
                                                    where r.EventRegistrationID == eventRegistration.ID
                                                    select r.EventScheduleID).ToList();

                decimal personNeedToPay = 0;
                if (aEvent.EventTypeID == 1)
                {
                    personNeedToPay = _entities.EventPrices.Single(a => a.EventID == eventID && a.EventActivityID == 8).UnitPrice;
                }

                int iMealCount = 0;
                foreach (string s in mValues)
                {
                    string s1 = " ";
                    foreach (int localRetreatScheduleID in mealBookingScheduleIDs)
                    {
                        if (localRetreatScheduleID.ToString() == s)
                        {
                            mealCounts[iMealCount]++;
                            s1 = "y";
                            int localRetreatActivityID = _entities.EventSchedules.Single(a => a.ID == localRetreatScheduleID).EventActivityID;
                            personNeedToPay += _entities.EventPrices.Single(a => a.EventActivityID == localRetreatActivityID && a.EventID == eventID).UnitPrice;
                            break;
                        }
                    }
                    row.Add(s1);
                    row.Add(String.Format("SGD{0:C}", personNeedToPay));
                    iMealCount++;
                }
                totalPrice += personNeedToPay;

                string remark = string.Empty;
                if (aEvent.EventTypeID == 5)
                {
                    if (mealBookingScheduleIDs.Count > 1)
                    {
                        totalGuests += mealBookingScheduleIDs.Count - 1;
                        remark = "Bring " + (mealBookingScheduleIDs.Count - 1).ToString() + " Guests";
                    }
                }
                else if (eventRegistration.BackDateTime != null && eventRegistration.BackDateTime < eventRegistration.Event.EndDateTime)
                {
                    remark = String.Format("{0:d MMM yyyy HH:mm }", eventRegistration.BackDateTime);
                }
                row.Add(remark);
                eventRegistrationList.Add(row);

                iRow++;
                row = new List<string>();
            }

            for (int k = 0; k < 6; k++)
            {
                row.Add(string.Empty);
            }
            for (int m = 0; m < mealCounts.Count(); m++)
            {
                row.Add(mealCounts[m].ToString());
            }
            if (aEvent.EventTypeID == 5)
            {
                int mealCount = eventRegistrations.Where(a => a.EventMealBookings.Count > 0).Count();
                mealCount += totalGuests;
                row.Add("Total Peoples: " + mealCount.ToString());
                if (totalGuests > 0)
                {
                    row.Add("Guests: " + totalGuests.ToString());
                }
            }
            else
            {
                row.Add(String.Format("SGD{0:C}", totalPrice));
                row.Add("(Total Collection )");
            }

            eventRegistrationList.Add(row);

            TempData["viewModel"] = eventRegistrationList;
            TempData["MealCounts"] = mealCounts.Count();
            TempData["mLabels"] = mLabels;

            ViewData["EventTitle"] = aEvent.Title;
            ViewData["StartDateTime"] = String.Format("{0:d MMM yyyy HH:mm}", aEvent.StartDateTime);
            ViewData["EndDateTime"] = String.Format("{0:d MMM yyyy HH:mm}", aEvent.EndDateTime);
            ViewData["RegistrationCloseDate"] = String.Format("{0:ddd, MMM d yyyy HH:mm}", aEvent.RegistrationCloseDate);
            ViewData["MealCounts"] = mealCounts.Count();
            ViewData["mLabels"] = mLabels;
            ViewData["EventTypeID"] = aEvent.EventTypeID;


            return View(eventRegistrationList);
        }

		public string GetMealTitles(IEnumerable<string> labels)
		{
			IEnumerator<string> labelEnumerator = labels.GetEnumerator();
			labelEnumerator.MoveNext();
			StringBuilder sb = new StringBuilder();

			foreach (string s in labels)
			{
				if (labelEnumerator.Current != null)
				{
					string temp = labelEnumerator.Current;
					if (temp.Contains("/"))
					{
						int spacePos = temp.IndexOf('/');
						temp = temp.Substring(spacePos + 1);
					}

					sb.Append("<th><font size = \"1\">");
					sb.AppendLine(temp);
					sb.Append("</font></th>");
				}
				labelEnumerator.MoveNext();
			}

			return sb.ToString();
		}

		private static void appendTitle3(StringBuilder sb, string cTitle, int count)
		{
			sb.Append("<th colspan= " + count + ">");
			sb.Append(cTitle);
			sb.Append("</th>");
		}

		public string GetMealDateTitles(IEnumerable<string> labels)
		{
			IEnumerator<string> labelEnumerator = labels.GetEnumerator();
			labelEnumerator.MoveNext();
			StringBuilder sb = new StringBuilder();

			int i = 0;
			int count = 0;
			string cTitle = null;
			for (i = 0; i < labels.Count(); i++)
			{
				string tTitle = labelEnumerator.Current.Substring(0, labelEnumerator.Current.IndexOf('/'));
				if (tTitle != cTitle)
				{
					if (cTitle != null)
					{
						count = i - count;
						appendTitle3(sb, cTitle, count);
					}
					cTitle = tTitle;
				}
				labelEnumerator.MoveNext();
			}
			appendTitle3(sb, cTitle, i - count);

			return sb.ToString();
		}

		public string[][] GetStringList()
		{
			string[][] headList = new string[][]
			{
				new string[] {"S/N", "Name in ID Card", "Member No", "ID Card No", "Contact No", "Volunteer Job", " Meals Selection", "To Pay", "Remark"},
                new string[] {},
                new string[] {},
			};

			string temps = GetMealDateTitles((List<string>)TempData["mLabels"]);
			headList[1] = FillHeaders(temps);
			temps = GetMealTitles((List<string>)TempData["mLabels"]);
			headList[2]  = FillHeaders(temps);

			return headList;
		}

		private static string[] FillHeaders(string temps)
		{
			//<th><font size = "1">Breakfast</font></th>
			temps = temps.Replace("<th colspan= 1>", "");
			temps = temps.Replace("</th>", "\t");
			temps = temps.Replace("<th><font size = \"1\">", "");
			temps = temps.Replace("</font>", "");
			temps = temps.TrimEnd('\t');
			string[] tempList = temps.Split('\t');

			return tempList;
		}

		public ActionResult GenerateExcel2()
		{
			var viewModel = (List<List<string>>)TempData["viewModel"];
			for (int i=0; i<viewModel.Count-1; i++)
			{
				viewModel[i][0] = (i+1).ToString(); 
			}
 			return this.Excel(null, viewModel.AsQueryable(), "LocalRetreatRegistrationTable.xls", GetStringList());
		}


		public List<string> GetHeaderList()
		{
			List<string> temp = new List<string>();
			temp.Add("Name");
			temp.Add("MemberNo");
			temp.Add("MemberFeeExpiredDate");
			temp.Add("DateOfInitiation");
			temp.Add("IDCardNo");
			temp.Add("ContactNo");
			temp.Add("Gender");

			return temp;
		}

		//    <tr >
		//    <th rowspan="3">S/N</th>
		//    <%--<th rowspan="3"></th>--%>
		//    <th rowspan="3">
		//        Name in ID Card</br> 识别证姓名
		//    </th>
		//    <th rowspan="3">
		//        ID Card No </br>识别证编号
		//    </th>
		//    <th rowspan="3">
		//        Contact No</br> 联络号码
		//    </th>

		//    <th rowspan="3">
		//       Volunteer Job Selection 打禅义工</br>
		//       DP = 护法</br>
		//       Clean = 清洁</br>
		//       Video = 音响&影像
		//     </th>

		//    <% int count = Model.FirstOrDefault().LocalRetreatMealBookingLabels.Count;
		//           if(count > 0){%>
		//    <th colspan=<%: count.ToString()%> >
		//        Meals Selection
		//    </th>

		//       <th rowspan="3">To Pay</th>

		//     <th rowspan="3">
		//        Remark</br>
		//        Early back time</br>
		//        提前离开时间
		//     </th>
		// <%} %>

		//     <%-- <th rowspan="3">Signature</th>
		//      <th rowspan="3">Name</th>--%>
		//</tr>
     
		//<tr >
		//     <%--  <th colspan= 4>05 Nov 2010 </th> <th colspan= 3>06 Nov 2010 </th>--%>
		//     <%= Html.GetMealDateTitles(Model.FirstOrDefault().LocalRetreatMealBookingLabels)%>
		//</tr>
   
		//<tr >
		//     <%-- <th>breakfast</th>  <th> lunch</th>  <th>dinner</th>   <th> midnight</th>  
		//     <th>breakfast</th> <th>lunch</th> <th>dinner</th>--%>
		//     <%= Html.GetMealTitles(Model.FirstOrDefault().LocalRetreatMealBookingLabels)%>
		//</tr>


        #region Details
        //
        // GET: /EventRegister/Details/5

		[Authorize]
		public ActionResult Details(int id)
        {
            List<string> vLabels = new List<string>();
            List<string> vValues = new List<string>();
            List<string> mLabels = new List<string>();
            List<string> mValues = new List<string>();

            // Show report to user, or when login in 
            GetEventVolunteerJobBooking(id, vLabels, vValues, "Remark");

            GetEventMealBooking(id, mLabels, mValues);

            EventRegistration eventRegistration = _entities.EventRegistrations.Single(a => a.ID == id);
            if (eventRegistration.Event.EventTypeID == 1)
            {
                // Add Blessing food:EventActivity.Name.StartsWith("Bless")
                int aEventID = _entities.EventRegistrations.Single(a => a.ID == id).EventID;
				EventPrice lra = _entities.EventPrices.Single(a => a.EventID == aEventID && a.EventActivityID == 8);
                mLabels.Add(lra.EventActivity.Name + "(SGD$" + lra.UnitPrice.ToString() + ")");
                mValues.Add("**");
            }

            var viewModel = new EventRegistrationViewModel
            {
                EventRegistration = _entities.EventRegistrations.Single(a => a.ID == id),
                EventVolunteerJobBookingLabels = vLabels,
                EventVolunteerJobBookingValues = vValues,
                LocalRetreatMealBookingLabels = mLabels,
                LocalRetreatMealBookingValues = mValues
            };

            return View(viewModel);
        }

        public void GetEventMealBooking(int id, List<string> mLabels, List<string> mValues)
        {
            var mealBookings = from r in _entities.EventMealBookings
                               where r.EventRegistrationID == id
                               select r;

            if (mealBookings != null)
            {
                foreach (EventMealBooking mb in mealBookings)
                {
                    mLabels.Add(mb.EventSchedule.EventActivity.Name + "(SGD$" +  _entities.EventPrices.Single(a=>a.EventID == mb.EventSchedule.EventID && a.EventActivityID == mb.EventSchedule.EventActivityID).UnitPrice.ToString() + ")");
                    mValues.Add(mb.EventSchedule.DateTimeFrom.Date.ToString("ddMMM"));
                }
            }

        }


        public void GetEventVolunteerJobBooking(int id, List<string> vLabels, List<string> vValues, string name)
        {

            List<EventVolunteerJobBooking> localRetreatVolunteerJobBookings = (from r in _entities.EventVolunteerJobBookings
                                                                               where r.EventRegistrationID == id
                                                                               orderby r.EventVolunteerJob.VolunteerJobTypeID, r.EventVolunteerJob.EventSchedule.DateTimeFrom
                                                                               select r).ToList();

            RetrieveEventVolunteerJobBooking(localRetreatVolunteerJobBookings, vLabels, vValues, name, null);
        }

        #endregion Details



        #region Create
        
        //
        // GET: /EventRegister/Create

		[Authorize]
		public ActionResult Create(int eventID, bool? forOthers, DateTime? backDateTime)
        {
            Event aEvent = _entities.Events.Single(a => a.ID == eventID);

            List<string> volunteerJobs;
            List<string> volunteerJobValues;
            GetVolunteerJob(eventID, out volunteerJobs, out volunteerJobValues, null);

            var viewModel = new EventRegistrationViewModel
            {
                EventRegistration = new EventRegistration(),
                EventVolunteerJobBookingLabels = volunteerJobs,
                EventVolunteerJobBookingValues = volunteerJobValues,
            };

            if (backDateTime == null)
            {
                backDateTime = _entities.Events.Single(a => a.ID == eventID).EndDateTime;
            }

			GetViewModel(eventID, aEvent.EventTypeID, viewModel);

            if (forOthers != null && forOthers == true)
            {
                var memberRegisterID = (from r in _entities.EventRegistrations where r.EventID == eventID select r.MemberID).ToList();
                var memberNotRegisterID = (from r in _entities.MemberInfos where !memberRegisterID.Contains(r.MemberID) && r.IsActive  orderby r.Name select r).ToList();
                ViewData["MemberInfo"] = memberNotRegisterID;
                viewModel.MemberInfo = memberNotRegisterID;
            }

            ViewData["BackDateTime"] = backDateTime;
            viewModel.EventRegistration.Event = aEvent;

            return View(viewModel);

        }

		private void GetViewModel(int eventID, int eventTypeID, EventRegistrationViewModel viewModel)
		{
			if (eventTypeID == 1 || eventTypeID == 5)
			{
				List<string> mealNameDates;
				List<string> mealNameDateValues;
				GetMealNameDate(eventID, out mealNameDates, out mealNameDateValues, true);
				viewModel.LocalRetreatMealBookingLabels = mealNameDates;
				viewModel.LocalRetreatMealBookingValues = mealNameDateValues;

                List<string> breakNameDates;
				List<string> breakNameDateValues;
				if (eventTypeID == 1)
				{
					GetMealNameDate(eventID, out breakNameDates, out breakNameDateValues, false);
					viewModel.EventBreakDateTimeLabels = breakNameDates;
					viewModel.EventBreakDateTimeValues = breakNameDateValues;
				}
			}
		}

        private void GetVolunteerJob(int eventID, out List<string> volunteerJobs, out List<string> volunteerJobValues, List<int> bookedVolunterJobs)
        {
            volunteerJobs = new List<string>();
            volunteerJobValues = new List<string>();
            var tVolunteerJobs = from a in _entities.EventVolunteerJobs
                                 where a.EventSchedule.Event.ID == eventID
                                 orderby a.VolunteerJobTypeID, a.EventSchedule.DateTimeFrom
                                 select a;

            int eventTypeID = _entities.Events.Single(a => a.ID == eventID).EventTypeID;
            if (tVolunteerJobs != null)
            {
                int c1 = 0;
                string temp = null;
                bool firstVideo = true;

                foreach (var c in tVolunteerJobs)
                {
                    if (c.PersonsNeeded - c.PersonsTaked > 0 || bookedVolunterJobs != null && bookedVolunterJobs.Contains(c.ID))
                    {
                        if (c.VolunteerJobType.Name == "Video" && eventTypeID == 1)
                        {
                            if (firstVideo)
                            {
                                temp = GetVideoStringStart(c);
                                c1 = c.ID;
                                firstVideo = false;
                            }
                            else
                            {
                                temp = GetVideoStringEnd(temp, c);
                                volunteerJobs.Add(temp);
                                volunteerJobValues.Add(c1.ToString() + "&" + c.ID.ToString());
                                firstVideo = true;
                            }
                        }
                        else
                        {
                            temp = c.VolunteerJobType.Remark + "/" + c.EventSchedule.DateTimeFrom.Date.ToString("ddMMM") + " " + c.EventSchedule.DateTimeFrom.ToString("HH:mm") + "-" +
                                c.EventSchedule.DateTimeFrom.AddHours(c.EventSchedule.ScheduleOffset.OffsetHours).ToString("HH:mm");
                            volunteerJobs.Add(temp);
                            volunteerJobValues.Add(c.ID.ToString());
                        }
                    }
                }
            }
        }

        private static string GetVideoStringEnd(string temp, EventVolunteerJob c)
        {
            temp += c.EventSchedule.DateTimeFrom.AddHours(c.EventSchedule.ScheduleOffset.OffsetHours).ToString("HH:mm");
            if (temp.EndsWith("00:00"))
            {
                temp = temp.Replace("00:00", "07:30");
            }
            return temp;
        }

        private static string GetVideoStringStart(EventVolunteerJob c)
        {
            string temp = c.VolunteerJobType.Remark + "/" + c.EventSchedule.DateTimeFrom.Date.ToString("ddMMM") + " " + c.EventSchedule.DateTimeFrom.ToString("HH:mm") + "-";
            if (temp.EndsWith("07:00-"))
            {
                temp = temp.Replace("07:00-", "07:30-");
            }
            return temp;
        }

        private void GetMealNameDate(int localRetreatID, out List<string> mealNameDates, out List<string> mealNameDateValues, bool meal)
        {
            mealNameDates = new List<string>();
            mealNameDateValues = new List<string>();
            var tMealNameDates = from a in _entities.EventPrices
                                 where a.EventActivityID != 8 && a.EventID == localRetreatID
                                 orderby a.EventActivityID
                                 select a;
            if (tMealNameDates != null)
            {
                foreach (var c in tMealNameDates)
                {
					EventSchedule eventSchedule = _entities.EventSchedules.SingleOrDefault(a => a.EventActivityID == c.EventActivityID && a.EventID == c.EventID);
                    string temp;
                    if (meal)
                    {
						temp = eventSchedule.DateTimeFrom.Date.ToString("dd MMM yyyy") + "/" + c.EventActivity.Name;
                    }
                    else
                    {
						temp = eventSchedule.DateTimeFrom.ToString();
                    }
                    mealNameDates.Add(temp);
					mealNameDateValues.Add(eventSchedule.ID.ToString());
                }
            }

        }

        private void GetBreakDateTime(int localRetreatID, out List<string> mealNameDates, out List<string> mealNameDateValues)
        {
            mealNameDates = new List<string>();
            mealNameDateValues = new List<string>();
			var tMealNameDates = from a in _entities.EventPrices
								 where a.EventActivityID != 8 && a.EventID == localRetreatID
								 orderby a.EventActivityID
								 select a;
            if (tMealNameDates != null)
            {
                foreach (var c in tMealNameDates)
                {
					EventSchedule eventSchedule = _entities.EventSchedules.Single(a => a.EventActivityID == c.EventActivityID && a.EventID == c.EventID);
					string temp = eventSchedule.DateTimeFrom.Date.ToString("dd MMM yyyy") + "/" + c.EventActivity.Name;
                    mealNameDates.Add(temp);
					mealNameDateValues.Add(eventSchedule.ID.ToString());
                }
            }

        }

        //
        // POST: /EventRegister/Create

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Create(EventRegistration eventRegistration, string[] MealBookingChecks, string[] VolunteerJobBookingChecks, string earlyBackDateTimeCheck, int eventID, Guid? MemberID, FormCollection collection)
        {

            if (MemberID == null)
            {
                MembershipUser muc = Membership.GetUser(User.Identity.Name);
                eventRegistration.MemberID = (Guid)muc.ProviderUserKey;
            }
            else
            {
                eventRegistration.MemberID = (Guid)MemberID;
            }

            eventRegistration.EventID = eventID;

			Event aEvent = _entities.Events.Single(a => a.ID == eventID);
            int eventTypeID = _entities.Events.Single(a => a.ID == eventID).EventTypeID;
            if (eventTypeID == 1 && !string.IsNullOrEmpty(earlyBackDateTimeCheck))
            {
                int id = int.Parse(earlyBackDateTimeCheck);
                EventSchedule lrs = _entities.EventSchedules.Single(a => a.ID == id);
				if (lrs.DateTimeFrom != aEvent.EndDateTime)
                {
                    eventRegistration.BackDateTime = lrs.DateTimeFrom;
                }
            }
            else
            {
				eventRegistration.BackDateTime = aEvent.EndDateTime;
            }


			if (_entities.EventRegistrations.Any(a => a.EventID == eventID && a.MemberID == eventRegistration.MemberID))
			{
				EventRegistration aEventRegistration = _entities.EventRegistrations.SingleOrDefault(a => a.MemberID == eventRegistration.MemberID && a.EventID == eventID);
				return View("RegisteredAlready", aEventRegistration);
			}
	
			try
            {

				if (_entities.EventRegistrations.Any(a => a.EventID == eventID && a.MemberID == eventRegistration.MemberID))
				{
					EventRegistration aEventRegistration = _entities.EventRegistrations.SingleOrDefault(a => a.MemberID == eventRegistration.MemberID && a.EventID == eventID);
					
				}

                if (eventTypeID == 1)
                {
					AddNewLocalRetreatMealBooking(eventRegistration, MealBookingChecks, aEvent.EndDateTime);
                }
				else if (eventTypeID == 5)
				{
					EventSchedule lrs = _entities.EventSchedules.Single(a => a.EventID == eventID);
					EventMealBooking localRetreatMealBooking = new EventMealBooking();
					localRetreatMealBooking.EventScheduleID = lrs.ID;
					eventRegistration.EventMealBookings.Add(localRetreatMealBooking);
                    int guests = 0;
                    if (aEvent.IsPublic && int.TryParse(collection["GuestNumbersTex"].ToString(), out guests) && guests > 0)
                    {
                        for (int i = 0; i < guests; i++)
                        {
                            localRetreatMealBooking = new EventMealBooking();
                            localRetreatMealBooking.EventScheduleID = lrs.ID;
                            eventRegistration.EventMealBookings.Add(localRetreatMealBooking);
                        }
                    }
				}

				AddNewEventVolunteerJobBooking(eventRegistration, VolunteerJobBookingChecks, aEvent.EndDateTime);
                if (!ModelState.IsValid)
                {
                    ViewData["errorMsg"] = ModelState.Values.Single(a => a.Errors.Count() > 0).Errors[0].ErrorMessage;
                    throw new Exception();
                }

				TempData["eventRegistration"] = eventRegistration;

                 return RedirectToAction("CreateConfirm");
            }
            catch
            {
                //Invalid - redisplay with errors

                List<string> volunteerJobs;
                List<string> volunteerJobValues;
                GetVolunteerJob(eventID, out volunteerJobs, out volunteerJobValues, null);

                var viewModel = new EventRegistrationViewModel
                {
                    EventRegistration = eventRegistration,
                    EventVolunteerJobBookingLabels = volunteerJobs,
                    EventVolunteerJobBookingValues = volunteerJobValues,
                };

                List<int> tValues = ConvertToIntList(VolunteerJobBookingChecks);

                ModelStateSetting(tValues, "VolunteerJobBookingChecks", eventTypeID);

				GetViewModel(eventID, eventTypeID, viewModel);
				if (eventTypeID == 1)// || aEvent.EventTypeID == 5)
                {
                    tValues = ConvertToIntList(MealBookingChecks);
                    ModelStateSetting(tValues, "MealBookingChecks", eventTypeID);
                }

                if (MemberID != null)
                {
                    var memberRegisterID = (from r in _entities.EventRegistrations where r.EventID == eventID select r.MemberID).ToList();
                    var memberNotRegisterID = (from r in _entities.MemberInfos where !memberRegisterID.Contains(r.MemberID) orderby r.Name select r).ToList();
                    ViewData["MemberInfo"] = memberNotRegisterID;
                    viewModel.MemberInfo = memberNotRegisterID;
                }

                viewModel.EventRegistration.Event = aEvent;

                return View(viewModel);
            }
        }

        private void AddNewLocalRetreatMealBooking(EventRegistration eventRegistration, string[] MealBookingChecks, DateTime backdatetime)
        {
            if (MealBookingChecks != null)
            {
                foreach (string mealBookingCheck in MealBookingChecks)
                {
                    int id = int.Parse(mealBookingCheck);
                    EventSchedule lrs = _entities.EventSchedules.Single(a => a.ID == id);
                    if (DateTime.Compare(lrs.DateTimeFrom, eventRegistration.BackDateTime.Value) <= 0)
                    {
                        EventMealBooking localRetreatMealBooking = new EventMealBooking();
                        localRetreatMealBooking.EventScheduleID = id;
                        eventRegistration.EventMealBookings.Add(localRetreatMealBooking);
                    }
                    else
                    {
                        ModelState.AddModelError("EventRegistration.MealBookingChecks", "Your booking is wrong!.");
                        return;
                        //throw new Exception(ModelState["EventRegistration.MealBookingChecks"].Errors.LastOrDefault().ErrorMessage);
                    }
                }
            }
        }

        private void AddNewEventVolunteerJobBooking(EventRegistration eventRegistration, string[] VolunteerJobBookingChecks, DateTime backdatetime)
        {

            if (VolunteerJobBookingChecks != null)
            {
                List<int> volunteerJobBookingChecks = ConvertToIntList(VolunteerJobBookingChecks);

                List<DateTime> dateTimes = new List<DateTime>();
                foreach (int id in volunteerJobBookingChecks)
                {
                    EventVolunteerJob eventVolunteerJob = _entities.EventVolunteerJobs.Single(a => a.ID == id);
                    EventSchedule lrs = _entities.EventSchedules.Single(a => a.ID == eventVolunteerJob.EventScheduleID);

                    if (DateTime.Compare(lrs.DateTimeFrom, eventRegistration.BackDateTime.Value) >= 0 && eventRegistration.BackDateTime < backdatetime)
                    {
                         ModelState.AddModelError("VolunteerJobBookingChecks", "Your should volunteer for a job that takes place before your leaving time!");
                         return;
                        //throw new Exception(ModelState["VolunteerJobBookingChecks"].Errors.LastOrDefault().ErrorMessage);
                    }
                    else if (dateTimes.Count() > 0 && dateTimes.Contains(lrs.DateTimeFrom))
                    {
                        ModelState.AddModelError("VolunteerJobBookingChecks", "Your can't booking two job at same time!");
                         return;
                        //throw new Exception(ModelState["VolunteerJobBookingChecks"].Errors.LastOrDefault().ErrorMessage);
                    }
                    else if (DateTime.Compare(lrs.DateTimeFrom, eventRegistration.BackDateTime.Value) <= 0 && eventVolunteerJob.PersonsNeeded > eventVolunteerJob.PersonsTaked)
                    {
                        EventVolunteerJobBooking localRetreatVolunteerJobBooking = new EventVolunteerJobBooking();
                        localRetreatVolunteerJobBooking.EventVolunteerJobID = id;
                        if (eventVolunteerJob.PersonsNeeded > 1)
                        {
                            localRetreatVolunteerJobBooking.Remark = (eventVolunteerJob.PersonsTaked + 1).ToString();
                        }
                        eventRegistration.EventVolunteerJobBookings.Add(localRetreatVolunteerJobBooking);
                    }
                    dateTimes.Add(lrs.DateTimeFrom);

                }

            }

        }

        private static List<int> ConvertToIntList(string[] BookingChecks)
        {
            List<int> BookingCheckList = new List<int>();

            if (BookingChecks != null)
            {
                foreach (var check in BookingChecks)
                {
                    if (check.Contains('&'))
                    {
                        string[] check1 = check.Split('&');
                        foreach (var i in check1)
                        {
                            BookingCheckList.Add(int.Parse(i));
                        }
                    }
                    else
                    {
                        BookingCheckList.Add(int.Parse(check));
                    }
                }
            }
            return BookingCheckList;
        }

        #endregion



        # region CreateConfirm
        //Get
		[Authorize]
        public ActionResult CreateConfirm()
        {
            EventRegistration eventRegistration = TempData["eventRegistration"] as EventRegistration;

			var viewModel = GetEventRegistrationViewModel(eventRegistration);

            TempData["eventRegistration"] = eventRegistration;

            return View(viewModel);
        }

		private EventRegistrationViewModel GetEventRegistrationViewModel(EventRegistration eventRegistration)
		{
			List<string> vLabels = new List<string>();
			List<string> vValues = new List<string>();
			List<string> mLabels = new List<string>();
			List<string> mValues = new List<string>();

			RetrieveEventVolunteerJobBooking(eventRegistration.EventVolunteerJobBookings.ToList(), vLabels, vValues, "Remark", null);

			var viewModel = new EventRegistrationViewModel
			{
				EventRegistration = eventRegistration,
				EventVolunteerJobBookingLabels = vLabels,
				EventVolunteerJobBookingValues = vValues,
			};

			int eventTypeID = _entities.Events.Single(a => a.ID == eventRegistration.EventID).EventTypeID;
			if (eventTypeID == 1 || eventTypeID == 5)
			{
				RetrieveLocalRetreatMealBooking(eventRegistration, mLabels, mValues);

				if (eventTypeID == 1)
				{
					EventPrice lra = _entities.EventPrices.Single(a => a.EventID == eventRegistration.EventID && a.EventActivityID == 8);
					mLabels.Add(lra.EventActivity.Name + "(SGD$" + lra.UnitPrice.ToString() + ")");
					mValues.Add("**");
				}

				viewModel.LocalRetreatMealBookingLabels = mLabels;
				viewModel.LocalRetreatMealBookingValues = mValues;

			}
			return viewModel;
		}

        private void RetrieveLocalRetreatMealBooking(EventRegistration eventRegistration, List<string> mLabels, List<string> mValues)
        {
			if (eventRegistration.EventMealBookings.Count > 0)
			{
				var mealBookings = from r in eventRegistration.EventMealBookings
								   select r;

				if (mealBookings != null)
				{
					foreach (EventMealBooking mb in mealBookings)
					{
						EventSchedule localRetreatSchedule = _entities.EventSchedules.Single(a => a.ID == mb.EventScheduleID);
						EventPrice eventPrice = _entities.EventPrices.Single(a => a.EventID == mb.EventRegistration.EventID && a.EventActivityID == localRetreatSchedule.EventActivityID);
						mLabels.Add(localRetreatSchedule.EventActivity.Name + "(SGD$" + eventPrice.UnitPrice.ToString() + ")");
						mValues.Add(localRetreatSchedule.DateTimeFrom.Date.ToString("ddMMM"));
					}
				}
			}

        }

        public void RetrieveEventVolunteerJobBooking(List<EventVolunteerJobBooking> EventVolunteerJobBookings, List<string> vLabels, List<string> vValues, string name, List<string> vPersonInCharge)
        {
            List<EventVolunteerJobBooking> eventVolunteerJobBookings = EventVolunteerJobBookings.OrderBy(a => a.EventVolunteerJobID).ToList();
            if (eventVolunteerJobBookings != null)
            {
                string temp = null;
                string temp_video = null;
                bool firstVideo = true;
                foreach (EventVolunteerJobBooking c in eventVolunteerJobBookings)
                {
                    EventVolunteerJob eventVolunteerJob = _entities.EventVolunteerJobs.Single(a => a.ID == c.EventVolunteerJobID);
                    if (eventVolunteerJob.VolunteerJobType.Name == "Video" && eventVolunteerJob.EventSchedule.Event.EventTypeID == 1)
                    {
                        if (firstVideo)
                        {
                            temp_video = GetVideoStringStart(eventVolunteerJob);
                            firstVideo = false;
                        }
                        else
                        {
                            temp_video = GetVideoStringEnd(temp_video, eventVolunteerJob);
                            AddVolunteerLabels(vLabels, name, eventVolunteerJob);
                            vValues.Add(temp_video);
                            if (vPersonInCharge != null)
                            {
                                vPersonInCharge.Add(c.EventRegistration.MemberInfo.Name);
                            }
                            firstVideo = true;
                        }
                    }
                    else
                    {
                        AddVolunteerLabels(vLabels, name, eventVolunteerJob);
                        if (eventVolunteerJob.VolunteerJobType.Name == "DP" && eventVolunteerJob.PersonsNeeded > 1)  // person need = 2 in one hour, need to cut two parts
                        {
                            int count = eventVolunteerJob.PersonsNeeded.Value;
                            double offsetHours = eventVolunteerJob.EventSchedule.ScheduleOffset.OffsetHours / count;

                            if (c.Remark == "1")
                            {
                                temp = eventVolunteerJob.EventSchedule.DateTimeFrom.Date.ToString("ddMMM") + " " + eventVolunteerJob.EventSchedule.DateTimeFrom.ToString("HH:mm") + "-" +
                                                        eventVolunteerJob.EventSchedule.DateTimeFrom.AddHours(offsetHours).ToString("HH:mm");
                            }
                            else
                            {
                                temp = eventVolunteerJob.EventSchedule.DateTimeFrom.Date.ToString("ddMMM") + " " + eventVolunteerJob.EventSchedule.DateTimeFrom.AddHours(offsetHours).ToString("HH:mm") + "-" +
                                                       eventVolunteerJob.EventSchedule.DateTimeFrom.AddHours(eventVolunteerJob.EventSchedule.ScheduleOffset.OffsetHours).ToString("HH:mm");
                            }
                        }
                        else
                        {
                            temp = eventVolunteerJob.EventSchedule.DateTimeFrom.Date.ToString("ddMMM") + " " + eventVolunteerJob.EventSchedule.DateTimeFrom.ToString("HH:mm") + "-" +
                                                   eventVolunteerJob.EventSchedule.DateTimeFrom.AddHours(eventVolunteerJob.EventSchedule.ScheduleOffset.OffsetHours).ToString("HH:mm");
                        }
                        vValues.Add(temp);
                        if (vPersonInCharge != null)
                        {
                            vPersonInCharge.Add(c.EventRegistration.MemberInfo.Name);
                        }
                    }

                }
            }
        }

        private static void AddVolunteerLabels(List<string> vLabels, string name, EventVolunteerJob c)
        {
            if (name == "Remark")
            {
                vLabels.Add(c.VolunteerJobType.Remark);
            }
            else
            {
                vLabels.Add(c.VolunteerJobType.Name);
            }
        }

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult CreateConfirm(FormCollection collection)
        {
				EventRegistration eventRegistration = TempData["eventRegistration"] as EventRegistration;

				if (_entities.EventRegistrations.Any(a => a.EventID == eventRegistration.EventID && a.MemberID == eventRegistration.MemberID))
				{
					EventRegistration aEventRegistration = _entities.EventRegistrations.SingleOrDefault(a => a.MemberID == eventRegistration.MemberID && a.EventID == eventRegistration.EventID);
					return View("RegisteredAlready", aEventRegistration);
				}
				else
				{
					try
					{
						eventRegistration.RegisterTime = DateTime.Now.ToUniversalTime().AddHours(8);
						eventRegistration.Event = null;
						_entities.AddToEventRegistrations(eventRegistration);

						UpdateEventVolunteerJob(eventRegistration);

						_entities.SaveChanges();
					}
					catch
					{
						var viewModel = GetEventRegistrationViewModel(eventRegistration);
						return View(viewModel);
					}
					
					EmailAcknowledgement(eventRegistration, true);

					var lastEventRegistration = (from c in _entities.EventRegistrations select c).OrderByDescending(x => x.ID).First();

					return RedirectToAction("Details", new { id = lastEventRegistration.ID });
					
				}
        }


		private void UpdateEventVolunteerJob(EventRegistration eventRegistration)
		{
			if (eventRegistration.EventVolunteerJobBookings != null)
			{
				foreach (EventVolunteerJobBooking c in eventRegistration.EventVolunteerJobBookings)
				{
					EventVolunteerJob eventVolunteerJob = _entities.EventVolunteerJobs.Single(a => a.ID == c.EventVolunteerJobID);
					eventVolunteerJob.PersonsTaked++;
					UpdateModel(eventVolunteerJob, "EventVolunteerJob");
				}
			}
		}

		public void EmailAcknowledgement(EventRegistration eventRegistration, bool register)
        {
            EmailMessage em = new EmailMessage();

			if (register)
			{
				em.Subject = eventRegistration.Event.Title + ' ' + eventRegistration.Event.EventType.Name + " Registration Acknowledgement";
				em.Message = "Thank you for Registering.\r\nTo check your Local retreat registrations, please go to http://smchsg.com/EventSignature/MyEvent?eventTypeID=1";
			}
			else
			{
				Event aEvent = _entities.Events.SingleOrDefault(a => a.ID == eventRegistration.EventID);
				em.Subject = "Your Registration for " + aEvent.Title + ' ' + aEvent.EventType.Name + " has been canceled.";
				em.Message = "To check your Local retreat registrations, please go to http://smchsg.com/EventSignature/MyEvent?eventTypeID=1";
			}

			em.To = Membership.GetUser(eventRegistration.MemberID).Email;
            em.bcc = "admin@smchsg.com";
            em.From = "admin@smchsg.com"; //Membership.GetUser(eventRegistration.Event.OrganizerNameID).Email;

			EmailService es = new EmailService();
			es.SendMessage(em);
        }

        # endregion
        private void ModelStateSetting(List<int> tValues, string checkString, int eventTypeID)
        {

            string[] sValues = new string[100];
            int i = 0;

            if (tValues.Count() != 0)
            {
                int t0 = 0;
                bool first = true;

                foreach (int t in tValues)
                {
                    if (checkString == "VolunteerJobBookingChecks" && _entities.EventVolunteerJobs.Single(a => a.ID == t).VolunteerJobType.Name == "Video" && eventTypeID == 1)
                    {
                        if (first)
                        {
                            t0 = t;
                            first = false;
                        }
                        else
                        {
                            sValues[i++] = t0.ToString() + '&' + t.ToString();
                            first = true;
                        }
                    }
                    else
                    {
                        sValues[i++] = t.ToString();
                    }
                }
            }

            ModelState.SetModelValue(checkString, new ValueProviderResult(sValues, "", CultureInfo.InvariantCulture));
        }



        # region Edit
        //
        //GET: /EventRegister/Edit/5
		[Authorize]
        public ActionResult Edit(int id)
        {
            EventRegistration eventRegistration = _entities.EventRegistrations.Single(a => a.ID == id);

            List<int> tValues = (from r in eventRegistration.EventVolunteerJobBookings
                                 orderby r.EventVolunteerJobID
                                 select r.EventVolunteerJobID).ToList();

            List<string> volunteerJobs;
            List<string> volunteerJobValues;
            GetVolunteerJob(eventRegistration.EventID, out volunteerJobs, out volunteerJobValues, tValues);

            var viewModel = new EventRegistrationViewModel
            {
                EventRegistration = eventRegistration,
                EventVolunteerJobBookingLabels = volunteerJobs,
                EventVolunteerJobBookingValues = volunteerJobValues,
            };

            int eventTypeID = _entities.Events.Single(a => a.ID == eventRegistration.EventID).EventTypeID;
 
            ModelStateSetting(tValues, "VolunteerJobBookingChecks", eventTypeID);

            if (eventTypeID == 1 || eventTypeID == 5)
            {
                List<string> mealNameDates;
                List<string> mealNameDateValues;
                GetMealNameDate(eventRegistration.EventID, out mealNameDates, out mealNameDateValues, true);

                viewModel = new EventRegistrationViewModel
                {
                    EventRegistration = eventRegistration,
                    EventVolunteerJobBookingLabels = volunteerJobs,
                    EventVolunteerJobBookingValues = volunteerJobValues,
                    LocalRetreatMealBookingLabels = mealNameDates,
                    LocalRetreatMealBookingValues = mealNameDateValues,
                };

                if (eventTypeID == 1)
				{
					List<string> breakNameDates;
					List<string> breakNameDateValues;
					GetMealNameDate(eventRegistration.EventID, out breakNameDates, out breakNameDateValues, false);
					if (eventRegistration.BackDateTime != null)
					{
						int backDateTimeID = _entities.EventSchedules.Single(a => a.DateTimeFrom == eventRegistration.BackDateTime.Value).ID;
						List<string> backDateTimeIDs = new List<string>();
						backDateTimeIDs.Add(backDateTimeID.ToString());
						ModelState.SetModelValue("earlyBackDateTimeCheck", new ValueProviderResult(backDateTimeIDs.ToArray(), "", CultureInfo.InvariantCulture));
					}
                    viewModel.EventBreakDateTimeLabels = breakNameDates;
                    viewModel.EventBreakDateTimeValues = breakNameDateValues;
                }
  
                 tValues = (from r in eventRegistration.EventMealBookings
                           select r.EventScheduleID).ToList();

                ModelStateSetting(tValues, "MealBookingChecks", eventTypeID);

            }

            return View(viewModel);
        }

        // POST: /EventRegister/Edit/5
        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Edit(int id, string[] MealBookingChecks, string[] VolunteerJobBookingChecks, string earlyBackDateTimeCheck)
        {
            EventRegistration eventRegistration = _entities.EventRegistrations.Single(a => a.ID == id);

            try
            {

                DateTime backTime = _entities.Events.Single(a => a.ID == eventRegistration.EventID).EndDateTime;
             
                int eventTypeID = _entities.Events.Single(a => a.ID == eventRegistration.EventID).EventTypeID;
                if (eventTypeID == 1 || eventTypeID == 5)
                {
                    eventRegistration.BackDateTime = backTime;
                    if (!string.IsNullOrEmpty(earlyBackDateTimeCheck))
                    {
                        int bID = int.Parse(earlyBackDateTimeCheck);
                        EventSchedule lrs = _entities.EventSchedules.Single(a => a.ID == bID);
                        if (lrs.DateTimeFrom != backTime)
                        {
                            eventRegistration.BackDateTime = lrs.DateTimeFrom;
                        }
                    }
  

                    if (eventRegistration.EventMealBookings.Count() > 0)
                    {
                        //delete the old ones
                        var localRetreatMealBookings = from r in _entities.EventMealBookings where r.EventRegistrationID == eventRegistration.ID select r;
                        DeleteMealBooking(localRetreatMealBookings);
                    }
                    // Add new ones
                    AddNewLocalRetreatMealBooking(eventRegistration, MealBookingChecks, backTime);
                }

                if (eventRegistration.EventVolunteerJobBookings.Count() > 0)
                {
                    // Delete all the record in LocalRetreatVoluteerJobBookings Table
                    var eventVolunteerJobBookings = from r in _entities.EventVolunteerJobBookings where r.EventRegistrationID == eventRegistration.ID select r;
                    DeleteVolunteerJobBooking(eventVolunteerJobBookings);
                }
                // Add new ones
                AddNewEventVolunteerJobBooking(eventRegistration, VolunteerJobBookingChecks, backTime);
                if (!ModelState.IsValid)
                {
                    ViewData["errorMsg"] = ModelState.Values.Single(a => a.Errors.Count() > 0).Errors[0].ErrorMessage;
                    throw new Exception();
                }


                eventRegistration.RegisterTime = DateTime.Now.ToUniversalTime().AddHours(8);

                UpdateModel(eventRegistration, "EventRegistration");

				UpdateEventVolunteerJob(eventRegistration);

                _entities.SaveChanges();

                return RedirectToAction("Details", new { id = id });

            }
            catch
            {

                List<string> volunteerJobs;
                List<string> volunteerJobValues;
                GetVolunteerJob(eventRegistration.EventID, out volunteerJobs, out volunteerJobValues, null);
    
                var viewModel = new EventRegistrationViewModel
                {
                    EventRegistration = eventRegistration,
                    EventVolunteerJobBookingLabels = volunteerJobs,
                    EventVolunteerJobBookingValues = volunteerJobValues,
                };

                List<int> tValues = ConvertToIntList(VolunteerJobBookingChecks);

                int eventTypeID = _entities.Events.Single(a => a.ID == eventRegistration.EventID).EventTypeID;
                ModelStateSetting(tValues, "VolunteerJobBookingChecks", eventTypeID);

                if (eventTypeID == 1)
                {
                    List<string> mealNameDates;
                    List<string> mealNameDateValues;
                    GetMealNameDate(eventRegistration.EventID, out mealNameDates, out mealNameDateValues, true);

                    List<string> breakNameDates;
                    List<string> breakNameDateValues;
                    GetMealNameDate(eventRegistration.EventID, out breakNameDates, out breakNameDateValues, false);

                    viewModel = new EventRegistrationViewModel
                    {
                        EventRegistration = eventRegistration,
                        EventVolunteerJobBookingLabels = volunteerJobs,
                        EventVolunteerJobBookingValues = volunteerJobValues,
                        LocalRetreatMealBookingLabels = mealNameDates,
                        LocalRetreatMealBookingValues = mealNameDateValues,
                        EventBreakDateTimeLabels = breakNameDates,
                        EventBreakDateTimeValues = breakNameDateValues,
                    };
                    tValues = ConvertToIntList(MealBookingChecks);

                    ModelStateSetting(tValues, "MealBookingChecks", eventTypeID);
                }

                return View(viewModel);
            }
        }

        # endregion




        # region Delete
        //
        // GET: /EventRegister/Delete/5
		[Authorize]
        public ActionResult Delete(int id)
        {
            var eventRegistration = _entities.EventRegistrations.Single(a => a.ID == id);
            return View(eventRegistration);
        }

        //
        // POST: /EventRegister/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var eventRegistration = _entities.EventRegistrations.Single(a => a.ID == id);

            ViewData["EventID"] = eventRegistration.EventID;
            DeleteEventRegistration(eventRegistration);
  
            _entities.SaveChanges();

			EmailAcknowledgement(eventRegistration, false);

            return View("Deleted");
        }

        public void DeleteEventRegistration(EventRegistration eventRegistration)
        {
            _entities.DeleteObject(eventRegistration);

            //if (eventRegistration.Event.EventTypeID == 1)
            {
                // Delete all the record in LocalRetreatMealBookings Table
                var localRetreatMealBookings = from r in _entities.EventMealBookings where r.EventRegistrationID == eventRegistration.ID select r;
                DeleteMealBooking(localRetreatMealBookings);
            }

            // Delete all the record in LocalRetreatVoluteerJobBookings Table
            var eventVolunteerJobBookings = from r in _entities.EventVolunteerJobBookings where r.EventRegistrationID == eventRegistration.ID select r;
            DeleteVolunteerJobBooking(eventVolunteerJobBookings);
        }

        private void DeleteMealBooking(IQueryable<EventMealBooking> localRetreatMealBookings)
        {
            foreach (EventMealBooking temp in localRetreatMealBookings)
            {
                _entities.DeleteObject(temp);
            }
        }

        private void DeleteVolunteerJobBooking(IQueryable<EventVolunteerJobBooking> pEventVolunteerJobBookings)
        {
            if (pEventVolunteerJobBookings.Count() > 0)
            {
                foreach (var plocalRetreatVolunteerJobBooking in pEventVolunteerJobBookings)
                {
                    _entities.EventVolunteerJobBookings.DeleteObject(plocalRetreatVolunteerJobBooking);
                    EventVolunteerJob eventVolunteerJob = _entities.EventVolunteerJobs.Single(a => a.ID == plocalRetreatVolunteerJobBooking.EventVolunteerJobID);
                    if (eventVolunteerJob.PersonsTaked >= 1)
                    {
                        eventVolunteerJob.PersonsTaked--;
                    }
                    UpdateModel(eventVolunteerJob, "EventVolunteerJob");
                }
				_entities.SaveChanges();
			}
        }
        # endregion

    }
}

