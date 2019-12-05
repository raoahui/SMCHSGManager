using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Globalization;


namespace SMCHSGManager.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class EventScheduleController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();

        //
        // GET: /EventSchedule/

		[Authorize]
		public ActionResult Table(int localRetreatID)
        {
            ViewData["LocalRetreatID"] = localRetreatID;
            var viewModel = from r in _entities.EventSchedules
                            where r.EventID == localRetreatID orderby r.DateTimeFrom
                            select r;

            return View(viewModel);
        }

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int localRetreatID)
        {
            ViewData["LocalRetreatID"] = localRetreatID;
            var viewModel = from r in _entities.EventSchedules
                            where r.EventID == localRetreatID
                            orderby r.DateTimeFrom  
                            select r;

            if (viewModel.Count() > 0)
            {
                return View(viewModel);
            }
            else
            {
                ViewData["Message"] = "There is no local retreat record in database, please use \"Create New\" button to create new one.";
                return View();
             }
        }

        //
   

        // GET: /EventSchedule/ScheduleModelSelect
		[Authorize(Roles = "Administrator")]
		public ActionResult ScheduleModelSelect()
        {
            List<SelectListItem> scheduleModelSelectLists = new List<SelectListItem>();
            List<int> scheduleModels = ((from r in _entities.LocalRetreatScheduleTemplates select r.Model).Distinct()).ToList();
            foreach (int i in scheduleModels)
            {
                SelectListItem item = new SelectListItem { Text = "Model" + i.ToString(), Value = i.ToString() };
                scheduleModelSelectLists.Add(item);
            }
            SelectListItem item1 = new SelectListItem { Text = "Manually Create", Value = (scheduleModels.Count() + 1).ToString() };
            scheduleModelSelectLists.Add(item1);

            ViewData["LocalRetreatScheduleModelSelectLists"] = scheduleModelSelectLists;

            return View();
        }


        // POST: /EventSchedule/ScheduleModelSelect
		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult ScheduleModelSelect(FormCollection collection)
        {
            Event curEvent = (Event)TempData["Event"];
            
            int ModelNo = int.Parse(collection.Get("LocalRetreatScheduleModel"));

            int count = ((from r in _entities.LocalRetreatScheduleTemplates select r.Model).Distinct()).Count();

            if (ModelNo != count + 1)
            {
                  GenerateLocalRetreatSchedules(curEvent, ModelNo);
               
                 return RedirectToAction("Index", "Event");
            }
            else
            {
                // manual create schedule:
                return RedirectToAction("Create", "EventSchedule", new { startDateTime = curEvent.StartDateTime, localRetreatID = curEvent.ID });
            }
        }

        public void GenerateEventSchedules(Event curEvent)
        {
            EventSchedule eventSchedule = new EventSchedule();
            eventSchedule.DateTimeFrom = curEvent.StartDateTime;
			eventSchedule.EventActivityID = 5;  // lunch
            TimeSpan temp = curEvent.EndDateTime - curEvent.StartDateTime;

            eventSchedule.ScheduleOffsetID = (from r in _entities.ScheduleOffsets where r.OffsetHours == temp.TotalHours select r.ID).FirstOrDefault();

			 //AddVolunteerJob(2, eventSchedule, "Clean");
			 //AddVolunteerJob(1, eventSchedule, "Video");

            eventSchedule.EventID = curEvent.ID;
            _entities.AddToEventSchedules(eventSchedule);

            _entities.SaveChanges();

        }

        //
        // GET: /EventSchedule/Create
		[Authorize(Roles = "Administrator")]
		public ActionResult Create(DateTime startDateTime, int localRetreatID)
        {
            List<string> volunteerJobLabels;
            List<string> volunteerJobValues;
            GetVolunteerJobInfo(out volunteerJobLabels, out volunteerJobValues);

            var viewModel = new EventScheduleViewModel
           {
               EventSchedule = new EventSchedule(),
               EventActivity = _entities.EventActivities.ToList(),
               ScheduleOffset = _entities.ScheduleOffsets.ToList(),
               VolunteerJobNameLabels = volunteerJobLabels,
               VolunteerJobNameValues = volunteerJobValues
           };

            if (startDateTime == null)
            {
                startDateTime = _entities.Events.Single(a => a.ID == localRetreatID).StartDateTime;
            }
            viewModel.EventSchedule.DateTimeFrom = startDateTime;
            viewModel.EventSchedule.ScheduleOffsetID = 2;   // 1 hour
 
            return View(viewModel);
        }

        private void GetVolunteerJobInfo(out List<string> volunteerJobLabels, out List<string> volunteerJobValues)
        {
            volunteerJobLabels = new List<string>();
            volunteerJobValues = new List<string>();

            if (_entities.VolunteerJobTypes.Count() > 0)
            {
                int i = 0;
                foreach (VolunteerJobType volunteerJobType in _entities.VolunteerJobTypes)
                {
                    volunteerJobLabels.Add(volunteerJobType.Remark);
                    volunteerJobValues.Add(i.ToString());
                }
            }
        }

        //
        // POST: /EventSchedule/Create
		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(int localRetreatID, DateTime startDateTime, EventSchedule EventSchedule, string[] VolunteerJobChecks)
        {
            try
            {
                EventSchedule.EventID = localRetreatID;
                EventSchedule.DateTimeFrom = startDateTime;

                 // add new
                AddVolumnJob2LocalRetreatVolunteerJobs(EventSchedule, VolunteerJobChecks);

                _entities.AddToEventSchedules(EventSchedule);
                _entities.SaveChanges();

                DateTime endDateTime = _entities.Events.Single(a => a.ID == localRetreatID).EndDateTime;
                DateTime DateTimeTo = EventSchedule.DateTimeFrom.AddHours(EventSchedule.ScheduleOffset.OffsetHours);

                if (DateTime.Compare(DateTimeTo, endDateTime) < 0)
                {
                    return RedirectToAction("Create", new { startDateTime = DateTimeTo, localRetreatID = localRetreatID });
                }

                AddCleaner2LocalRetreatVolunteerJobs(localRetreatID, DateTimeTo);

                return RedirectToAction("Index", new { localRetreatID = localRetreatID });
            }
            catch
            {
                //Invalid - redisplay with errors
                List<string> volunteerJobLabels;
                List<string> volunteerJobValues;
                GetVolunteerJobInfo(out volunteerJobLabels, out volunteerJobValues);

                var viewModel = new EventScheduleViewModel
                {
                    EventSchedule = EventSchedule,
					EventActivity = _entities.EventActivities.ToList(),
                    ScheduleOffset = _entities.ScheduleOffsets.ToList(),
                    VolunteerJobNameLabels = volunteerJobLabels,
                    VolunteerJobNameValues = volunteerJobValues,
                };

                return View(viewModel);
            }
        }

        //public void GenerateEventSchedules(Event aEvent, int modelNo)
        //{
        //    if (aEvent.EventTypeID == 1)
        //    {
        //        GenerateLocalRetreatSchedules(aEvent, modelNo);
        //    }
        //    else
        //    {
        //        EventSchedule eventSchedule = new EventSchedule();
        //        eventSchedule.DateTimeFrom = aEvent.StartDateTime;
        //        eventSchedule.EventActivityID = aEvent.EventActivities.FirstOrDefault().ID;
        //        TimeSpan temp = aEvent.EndDateTime - aEvent.StartDateTime;

        //        eventSchedule.ScheduleOffsetID = (from r in _entities.ScheduleOffsets where r.OffsetHours == temp.TotalHours select r.ID).FirstOrDefault();

        //        if (aEvent.EventTypeID == 2 || aEvent.EventTypeID == 3)
        //        {
        //            AddVolunteerJob(1, eventSchedule, "DP");
        //        }
        //        else
        //        {
        //            AddVolunteerJob(2, eventSchedule, "Clean");
        //        }
        //        AddVolunteerJob(1, eventSchedule, "Video");

        //        eventSchedule.EventID = aEvent.ID;
        //        _entities.AddToEventSchedules(eventSchedule);
        //    }

        //    _entities.SaveChanges();
        //}


        public void GenerateLocalRetreatSchedules(Event aEvent, int modelNo)
        {
            DateTime dateTimeFrom = aEvent.StartDateTime;

            List<LocalRetreatScheduleTemplate> localRetreatScheduleTemplates = (from r in _entities.LocalRetreatScheduleTemplates where r.Model == modelNo orderby r.ID select r).ToList();
            foreach (LocalRetreatScheduleTemplate localRetreatScheduleTemplate in localRetreatScheduleTemplates)
            {
                int presetLocalRetreatScheduleOffsetID = localRetreatScheduleTemplate.ScheduleOffsetID;

                EventSchedule localRetreatSchedule = new EventSchedule();
                localRetreatSchedule.DateTimeFrom = dateTimeFrom;

                //int startActivityID = (from r in _entities.EventActivities where r.EventID == aEvent.ID orderby r.ID select r.ID).FirstOrDefault();
                localRetreatSchedule.EventActivityID = localRetreatScheduleTemplate.EventActivityID;

                dateTimeFrom = dateTimeFrom.AddHours(_entities.ScheduleOffsets.Single(a => a.ID == presetLocalRetreatScheduleOffsetID).OffsetHours);
                if (DateTime.Compare(dateTimeFrom, aEvent.EndDateTime) > 0)
                {
                    presetLocalRetreatScheduleOffsetID--;
                    dateTimeFrom = aEvent.EndDateTime; // dateTimeFrom.AddHours(_entities.LocalRetreatScheduleOffsets.Single(a => a.ID == presetLocalRetreatScheduleOffsetID).OffsetHours);
                }

                localRetreatSchedule.ScheduleOffsetID = presetLocalRetreatScheduleOffsetID;

                AddVolunteerJob(localRetreatScheduleTemplate.DP_PersonNeeded, localRetreatSchedule, "DP");
                AddVolunteerJob(localRetreatScheduleTemplate.Clean_PersonNeeded, localRetreatSchedule, "Clean");
                AddVolunteerJob(localRetreatScheduleTemplate.Video_PersonNeeded, localRetreatSchedule, "Video");

                //aEvent.EventSchedules.Add(localRetreatSchedule);
                localRetreatSchedule.EventID = aEvent.ID;
                _entities.AddToEventSchedules(localRetreatSchedule);

                if (DateTime.Compare(dateTimeFrom, aEvent.EndDateTime) == 0)
                {
                    break;
                }
            }

            EventSchedule lastLocalRetreatSchedule = new EventSchedule();
            lastLocalRetreatSchedule.DateTimeFrom = dateTimeFrom;
            lastLocalRetreatSchedule.EventActivityID = 8;    // "bless"
            lastLocalRetreatSchedule.ScheduleOffsetID = 1;   // 0.5 hour

            AddVolunteerJob(2, lastLocalRetreatSchedule, "Clean");
            //aEvent.EventSchedules.Add(lastLocalRetreatSchedule);
            lastLocalRetreatSchedule.EventID = aEvent.ID;
            _entities.AddToEventSchedules(lastLocalRetreatSchedule);

            _entities.SaveChanges();
        }

        public void AddVolunteerJob(int personNeededs, EventSchedule localRetreatSchedule, string flag)
        {
            if (personNeededs > 0)
            {
                EventVolunteerJob localRetreatVolunteerJob = new EventVolunteerJob();
                localRetreatVolunteerJob.VolunteerJobTypeID = _entities.VolunteerJobTypes.Single(a => a.Name.StartsWith(flag)).ID;
                localRetreatVolunteerJob.PersonsNeeded = personNeededs;
                localRetreatVolunteerJob.PersonsTaked = 0;
                localRetreatSchedule.EventVolunteerJobs.Add(localRetreatVolunteerJob);
            }
        }

        private static void AddVolumnJob2LocalRetreatVolunteerJobs(EventSchedule newLocalRetreatSchedule, string[] VolunteerJobChecks)
        {
            // Add VolunteerJob to localRetreatVolunteerJobs Table
            int volumnJobID = 0;
            foreach (string s in VolunteerJobChecks)
            {
                volumnJobID++;
                int personsNeeded = int.Parse(s);
                if (personsNeeded > 0)
                {
                    EventVolunteerJob localRetreatVolunteerJob = new EventVolunteerJob();
                    localRetreatVolunteerJob.VolunteerJobTypeID = volumnJobID;
                    localRetreatVolunteerJob.PersonsNeeded = personsNeeded;
                    localRetreatVolunteerJob.PersonsTaked = 0;
                    newLocalRetreatSchedule.EventVolunteerJobs.Add(localRetreatVolunteerJob);
                }
            }

         }

        // this is for end of local retreat only:
        public void AddCleaner2LocalRetreatVolunteerJobs(int localRetreatID, DateTime DateTimeTo)
        {
             
             EventSchedule lastLocalRetreatSchedule = new EventSchedule();
             lastLocalRetreatSchedule.DateTimeFrom = DateTimeTo;
             lastLocalRetreatSchedule.EventID = localRetreatID;
             lastLocalRetreatSchedule.EventActivityID = 8;    // "bless"
             lastLocalRetreatSchedule.ScheduleOffsetID = 1;

             AddVolunteerJob(2, lastLocalRetreatSchedule, "Clean");
             //EventVolunteerJob localRetreatVolunteerJob = new EventVolunteerJob();
             //localRetreatVolunteerJob.VolunteerJobTypeID = CleanID;   // "Cleaner"
             //localRetreatVolunteerJob.PersonsNeeded = 2;
             //localRetreatVolunteerJob.PersonsTaked = 0;
             //lastLocalRetreatSchedule.EventVolunteerJobs.Add(localRetreatVolunteerJob);

             _entities.AddToEventSchedules(lastLocalRetreatSchedule);
             _entities.SaveChanges();

        }

        //
        // GET: /EventSchedule/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {

            List<string> volunteerJobLabels;
            List<string> volunteerJobValues;
            GetVolunteerJobInfoFromBefore(id, out volunteerJobLabels, out volunteerJobValues);

            EventSchedule eventSchedule = _entities.EventSchedules.Single(a => a.ID == id);
            int eventID = eventSchedule.EventID;
            var viewModel = new EventScheduleViewModel
            {
                EventSchedule = eventSchedule,
				EventActivity = _entities.EventActivities.ToList(),
                ScheduleOffset = _entities.ScheduleOffsets.ToList(),
                VolunteerJobNameLabels = volunteerJobLabels,
                VolunteerJobNameValues = volunteerJobValues
           };

            return View(viewModel);
        }

        private void GetVolunteerJobInfoFromBefore(int id, out List<string> volunteerJobLabels, out List<string> volunteerJobValues)
        {
            volunteerJobLabels = new List<string>();
            volunteerJobValues = new List<string>();

            var previousVolunteerJobs = from r in _entities.EventVolunteerJobs
                                        where r.EventScheduleID == id
                                        select r;

            if (_entities.VolunteerJobTypes.Count() > 0)
            {
                foreach (VolunteerJobType t in _entities.VolunteerJobTypes)
                {
                    volunteerJobLabels.Add(t.Remark);
                    int personsNeeded = 0;
                    foreach (EventVolunteerJob pVolunteerJob in previousVolunteerJobs)
                    {
                        if (pVolunteerJob.VolunteerJobTypeID == t.ID)
                        {
                            personsNeeded = (int)pVolunteerJob.PersonsNeeded;
                            break;
                        }
                    }
                    volunteerJobValues.Add(personsNeeded.ToString());
                }
            }
        }

        //
        // POST: /EventSchedule/Edit/5
		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection, string[] VolunteerJobChecks)
        {
            var localRetreatSchedule = _entities.EventSchedules.Single(a => a.ID == id);

            try
            {
                 //UpdateModel(localRetreatSchedule, "localRetreatSchedule");

                var oldVolunteerJobs = from r in _entities.EventVolunteerJobs
                                       where r.EventScheduleID == localRetreatSchedule.ID
                                       select r;

                //remove the old ones 
                if (oldVolunteerJobs.Count() > 0)
                {
                    foreach (var oldVolunteerJob in oldVolunteerJobs)
                    {
                        _entities.EventVolunteerJobs.DeleteObject(oldVolunteerJob);
                    }
                }

                //Add new
                AddVolumnJob2LocalRetreatVolunteerJobs(localRetreatSchedule, VolunteerJobChecks);

                UpdateModel(localRetreatSchedule, "localRetreatSchedule");
                _entities.SaveChanges();

                return RedirectToAction("Index", new { localRetreatID = localRetreatSchedule.EventID });
            }
            catch
            {
                //Invalid - redisplay with errors
                List<string> volunteerJobLabels;
                List<string> volunteerJobValues;
                GetVolunteerJobInfoFromBefore(id, out volunteerJobLabels, out volunteerJobValues);

                var viewModel = new EventScheduleViewModel
                {
                    EventSchedule = localRetreatSchedule,
					EventActivity = _entities.EventActivities.ToList(),
                    ScheduleOffset = _entities.ScheduleOffsets.ToList(),
               };

                return View(viewModel);
            }
        }

        //
        // GET: /EventSchedule/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
            var localRetreatSchedule = _entities.EventSchedules.Single(a => a.ID == id);
            return View(localRetreatSchedule);
        }

        //
        // POST: /EventSchedule/Delete/5

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                var localRetreatSchedule = _entities.EventSchedules.Include("EventActivity").Single(a => a.ID == id);
                ViewData["LocalRetreatID"] = localRetreatSchedule.EventID;
                _entities.DeleteObject(localRetreatSchedule);

                // Delete all the record in LocalRetreatVolunteerJobs Table
                var localRetreatVolunteerJobs = from r in _entities.EventVolunteerJobs where r.EventScheduleID == id select r;
                if (localRetreatVolunteerJobs != null)
                {
                    foreach (EventVolunteerJob temp in localRetreatVolunteerJobs)
                    {
                        _entities.DeleteObject(temp);
                    }
                }

                _entities.SaveChanges();
            }
            return View("Deleted");
        }

    }
}
