using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;
using System.Globalization;
using System.IO;
using System.Text;

namespace SMCHSGManager.Controllers
{
    public class EventController : Controller
    {

        private SMCHDBEntities _entities = new SMCHDBEntities();
        private int _pageSizeUpcoming = 3;
        private int _pageSizeRecent = 3;
        //private int _pageSize = 15;//500;
        //
        // GET: /Event/

        public ActionResult Index(int? pageUpcoming, int? pageRecent)
        {
            var currentPageUpcoming = pageUpcoming ?? 1;
            ViewData["CurrentPageUpcoming"] = currentPageUpcoming;
            ViewData["PageSizeUpcoming"] = _pageSizeUpcoming;

            var currentPageRecent = pageRecent ?? 1;
            ViewData["CurrentPageRecent"] = currentPageRecent;
            ViewData["PageSizeRecent"] = _pageSizeRecent;

            bool initiateOnly = User.IsInRole("Initiate");
            var upcomingViewModel = GetUpcomingEvents(true, initiateOnly);
            var recentViewModel = GetUpcomingEvents(false, initiateOnly);
 
            ViewData["TotalPagesUpcoming"] = (int)Math.Ceiling((float)upcomingViewModel.Count() / _pageSizeUpcoming);
            ViewData["TotalPagesRecent"] = (int)Math.Ceiling((float)recentViewModel.Count() / _pageSizeRecent);

            var pageViewModel = new EventListViewModel
            {
                UpcomingEvents = upcomingViewModel.Skip((currentPageUpcoming - 1) * _pageSizeUpcoming).Take(_pageSizeUpcoming).ToList(),
                RecentEvents = recentViewModel.Skip((currentPageRecent - 1) * _pageSizeRecent).Take(_pageSizeRecent).ToList(),
            };

			pageViewModel.UpcomingEventsImages = GetEventImages(pageViewModel.UpcomingEvents);
			pageViewModel.RecentEventsImages = GetEventImages(pageViewModel.RecentEvents);

            return View(pageViewModel);
        }

        public IOrderedQueryable<Event> GetUpcomingEvents(bool future, bool initiateOnly)
        {
            var viewModel = from r in _entities.Events where r.EventTypeID != 2 && r.EventTypeID != 3 select r;
            if (!initiateOnly)
            {
                viewModel = from r in _entities.Events where r.EventTypeID != 2 && r.EventTypeID != 3 && r.IsPublic select r;
            }

            DateTime today = DateTime.Today.ToUniversalTime().AddHours(8);
            var viewModel1 = from r in viewModel where r.StartDateTime > today orderby r.StartDateTime ascending select r;
            if (!future)
            {
                viewModel1 = from r in viewModel where r.StartDateTime <= today orderby r.StartDateTime descending select r;
            }
            return viewModel1;
        }


		public string[] GetEventImages(List<Event> aEvents)
		{
			string[] Images = new string[6];
			int i = 0;
			foreach (Event an in aEvents)
			{
				if (an != null && an.EventUploadFiles != null)
				{
					foreach (UploadFile uploadFile in an.EventUploadFiles.Select(a => a.UploadFile).ToList())
					{
						if (uploadFile.ContentType.StartsWith("image"))
						{
							Images[i] = uploadFile.FilePath + uploadFile.Name; 
							break;
						}
					}

					i++;
				}
			}
			return Images;
		}

  
        //
        // GET: /Event/Details/5

        public ActionResult Details(int id)
        {
             var aEvent = _entities.Events.Single(a => a.ID == id);

            var events = from r in _entities.Events where r.EventTypeID != 2 && r.EventTypeID != 3 orderby r.StartDateTime descending select r;
            if (!User.IsInRole("Initiate"))
            {
                 events = from r in _entities.Events where r.EventTypeID != 2 && r.EventTypeID != 3 && r.IsPublic orderby r.StartDateTime descending select r;
            }

            DateTime today = DateTime.Today.ToUniversalTime().AddHours(8);
            var eventIDs = (from r in events where r.StartDateTime <= today orderby r.StartDateTime descending select r.ID).ToList();
            if (aEvent.StartDateTime > today)
            {
                eventIDs = (from r in events where r.StartDateTime > today orderby r.StartDateTime descending select r.ID).ToList();
            }

            var viewModel = new EventViewModel
            {
                Event = _entities.Events.Single(a => a.ID == id),
                EventIDs = eventIDs,
            };
            return View(viewModel);
        }

        //
        // GET: /Event/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            var viewModel = new EventViewModel()
            {
                Event = new Event(),
                Locations = _entities.Locations.ToList(),
                EventTypes = (from r in _entities.EventTypes where r.ID != 2 && r.ID != 3 select r) .ToList(),
            };

            viewModel.Event.Title = "[Please input event title]";
            DateTime today = DateTime.Today.ToUniversalTime().AddHours(8);
            viewModel.Event.StartDateTime = today.AddDays(10).AddHours(22).AddMinutes(30);
            viewModel.Event.EndDateTime = viewModel.Event.StartDateTime.AddDays(1).AddHours(12);
            viewModel.Event.RegistrationOpenDate = today.AddDays(-20);
            viewModel.Event.RegistrationCloseDate = today.AddDays(-7);

            viewModel.Event.IsPublic = false;

            //viewModel.LocalRetreatScheduleModelSelectLists = GetScheduleSelectLists();

            return View(viewModel);
        }

        protected void ValidateEvent(Event Event)
        {
            if (Event.EndDateTime < Event.StartDateTime )
            {
                ModelState.AddModelError("Event.StartDateTime", "StartDateTime should be earlier than EndDateTime!.");
            }
            if (Event.EventTypeID == 5 )
            {
                TimeSpan temp = Event.EndDateTime - Event.StartDateTime;
                int ScheduleOffsetID = (from r in _entities.ScheduleOffsets where r.OffsetHours == temp.TotalHours select r.ID).FirstOrDefault();
                if (ScheduleOffsetID == 0)
                {
                    ModelState.AddModelError("Event.EndDateTime", "EndDateTime or StartDateTime not correct!.");
                }
            }
            //if (!ModelState.IsValid)
            //{
            //    throw new Exception(ModelState["Event.StartDateTime"].Errors.LastOrDefault().ErrorMessage);
            //} 
            if (Event.RegistrationCloseDate < Event.RegistrationOpenDate)
            {
                ModelState.AddModelError("Event.RegistrationOpenDate", "RegistrationOpenDate should be earlier than RegistrationCloseDate!.");
            }
            //if (!ModelState.IsValid)
            //{
            //    throw new Exception(ModelState["Event.RegistrationOpenDate"].Errors.LastOrDefault().ErrorMessage);
            //}            
            if (Event.RegistrationCloseDate > Event.StartDateTime)
            {
                ModelState.AddModelError("Event.RegistrationCloseDate", "RegistrationCloseDate  should be earlier than StartDateTime!.");
            }
            //if (!ModelState.IsValid)
            //{
            //    throw new Exception(ModelState["Event.RegistrationCloseDate"].Errors.LastOrDefault().ErrorMessage);
            //}
        }

        
        //
        // POST: /Event/Create

		//[HttpPost, ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, Event Event, string Upload)
        {
			MembershipUser mu = Membership.GetUser();
			
			try
            {
                ValidateEvent(Event);
                if (!ModelState.IsValid)
                {
                    throw new Exception();//ModelState["Event.StartDateTime"].Errors.Last().ErrorMessage);
                } 

                ImageController ac = new ImageController();

				List<UploadFile> uploadFiles = ac.GenerateAttachFileCollectionID(collection, "Event");

                if (Upload != null)
                {
                    ImageUploadToServer();
					if (uploadFiles != null)
                    {
						ModelStateSetting(uploadFiles, "Event");
                    } 
                    var viewModel = new EventViewModel()
                    {
                        Event = Event,
                        Locations = _entities.Locations.ToList(),
                        EventTypes = (from r in _entities.EventTypes where r.ID != 2 && r.ID != 3 select r).ToList(),
                    };
                    return View(viewModel);
                }

				foreach (UploadFile uploadFile in uploadFiles)
				{
					EventUploadFile auf = new EventUploadFile();
					auf.UploadFileID = uploadFile.ID;
					Event.EventUploadFiles.Add(auf);
				}

				Event.OrganizerNameID = (Guid)(mu.ProviderUserKey);
				
				//GenerateEventPrices(Event);
   
                _entities.AddToEvents(Event);
                _entities.SaveChanges();


                if (Event.EventTypeID == 1)
                {
                    Event = (from r in _entities.Events select r).OrderByDescending(x => x.ID).First();
                    TempData["Event"] = Event;

                    return RedirectToAction("ScheduleModelSelect", "EventSchedule");
                }
                else
                {
                    EventScheduleController esc = new EventScheduleController();
                    esc.GenerateEventSchedules(Event);
  
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                var viewModel = new EventViewModel()
                {
                    Event = new Event(),
                    Locations = _entities.Locations.ToList(),
                    EventTypes = (from r in _entities.EventTypes where r.ID != 2 && r.ID != 3 select r).ToList(),
                };
                //viewModel.LocalRetreatScheduleModelSelectLists = GetScheduleSelectLists();

                return View(viewModel);
            }
        }

		public void ModelStateSetting(List<UploadFile> uploadFiles, string ControllerName)
		{
			foreach (UploadFile uploadFile in uploadFiles)
			{
				ModelState.SetModelValue(ControllerName + ".&." + uploadFile.ID.ToString(), new ValueProviderResult("True", "", CultureInfo.InvariantCulture));
			}
		}
		
        public void AdjustLocalRetreatSchedules(Event aEvent, int baseActivityID)
        {
            foreach (EventSchedule ac in aEvent.EventSchedules)
            {
                ac.EventActivityID += baseActivityID;
            }
        }

        //public void GenerateEventPrices(Event aEvent)
        //{
        //    if (aEvent.EventTypeID == 1)
        //    {
        //        //NewEventPrice(aEvent, 4, 1.5M);
        //        //NewEventPrice(aEvent, 5, 5M);
        //        //NewEventPrice(aEvent, 6, 5M);
        //        //NewEventPrice(aEvent, 8, 1M);
        //        NewEventPrice(aEvent, 4, 5M);
        //        NewEventPrice(aEvent, 8, 2M);
        //    }
        //    else if (aEvent.EventTypeID == 5)
        //    {
        //        NewEventPrice(aEvent, 5, 10M);
        //    }
        //}

        //private static void NewEventPrice(Event aEvent, int eventActivityID, decimal unitPrice)
        //{
        //    EventPrice eventPrice = new EventPrice();
        //    eventPrice.EventActivityID = eventActivityID;
        //    eventPrice.UnitPrice = unitPrice;
        //    aEvent.EventPrices.Add(eventPrice);
        //}


        //
        // GET: /Event/Edit/5
 
        public ActionResult Edit(int id)
        {
            var aEvent = _entities.Events.Single(a => a.ID == id);

            if (aEvent.EventUploadFiles != null)
            {
				List<UploadFile> uploadFiles = aEvent.EventUploadFiles.Select(a => a.UploadFile).ToList();
				ModelStateSetting(uploadFiles, "Event");
            }

            var viewModel = new EventViewModel()
            {
                Event = aEvent,
                Locations = _entities.Locations.ToList(),
                EventTypes = (from r in _entities.EventTypes where r.ID != 2 && r.ID != 3 select r).ToList(),
            };
            return View(viewModel);
        }

        //
        // POST: /Event/Edit/5

		//[HttpPost, ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection, string Upload)
        {
            var aEvent = _entities.Events.Single(a => a.ID == id);
             
            try
            {
                ImageController ac = new ImageController();
				List<UploadFile> uploadFiles = ac.GenerateAttachFileCollectionID(collection, "Event");

                if (Upload != null)
                {
                    ImageUploadToServer();

					if (uploadFiles != null)
                    {
						ModelStateSetting(uploadFiles, "Event");
                    }

                    UpdateModel(aEvent, "Event");
                    var viewModel = new EventViewModel()
                    {
                        Event = aEvent,
                        Locations = _entities.Locations.ToList(),
                        EventTypes = (from r in _entities.EventTypes where r.ID != 2 && r.ID != 3 select r).ToList(),
                    };
                    return View(viewModel);
                }

				List<EventUploadFile> aEventUploadFiles = aEvent.EventUploadFiles.ToList();
				foreach (EventUploadFile aEventUploadFile in aEventUploadFiles)
				{
					aEvent.EventUploadFiles.Remove(aEventUploadFile);
				}

				foreach (UploadFile uploadFile in uploadFiles)
				{
					EventUploadFile auf = new EventUploadFile();
					auf.UploadFileID = uploadFile.ID;
					aEvent.EventUploadFiles.Add(auf);
				}

                UpdateModel(aEvent, "Event");
                _entities.SaveChanges(); 
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void ImageUploadToServer()
        {
            HttpPostedFileBase file = Request.Files["fileUpload"];
            if (string.IsNullOrEmpty(file.FileName))
            {
                throw new Exception("Please select a file!");
            }

			ImageController ic = new ImageController();
			string savePath = "../images/UploadFiles";
			string filePath = Path.Combine(HttpContext.Server.MapPath(savePath), Path.GetFileName(file.FileName));
			UploadFile uploadFile = ic.ImageUpload(file, filePath, savePath);
			ViewData["CurrentUploadFile"] = uploadFile;
        }


        [Authorize, HttpPost]
        public ActionResult Register(int id) {

            MembershipUser muc = Membership.GetUser(User.Identity.Name);

            EventRegistration eventRegistration = new EventRegistration();
            eventRegistration.MemberID = (Guid)muc.ProviderUserKey;

            eventRegistration.EventID = id;
            eventRegistration.RegisterTime = DateTime.Now.ToUniversalTime().AddHours(8);
            _entities.AddToEventRegistrations(eventRegistration);
             
            _entities.SaveChanges();
  
            return Content("Thanks - we'll see you there!");
        }


        //
        // GET: /Event/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
            var aEvent = _entities.Events.Single(a => a.ID == id);
            return View(aEvent);
        }

        //
        // POST: /Event/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                var aEvent = _entities.Events.Single(a => a.ID == id);

				//// Delete all the record in EventRegistration Table
				//var eventRegistrations = from r in _entities.EventRegistrations where r.EventID == id select r;
				//if (eventRegistrations != null)
				//{
				//    EventRegistrationController erc = new EventRegistrationController();
				//    foreach (EventRegistration eventRegistration in eventRegistrations)
				//    {
				//        DeleteEventRegistration(eventRegistration);
				//    }
				//}


       
                _entities.DeleteObject(aEvent);

				////delete all record in EventActivity table
				//var eventActivities = from r in _entities.EventActivities where r.EventID == id select r;
				//{
				//    foreach (EventActivity eventActivity in eventActivities)
				//    {
				//        _entities.EventActivities.DeleteObject(eventActivity);
				//    }
				//}

				//// Delete all the record in EventSchedule Table
				//var eventSchedules = from r in _entities.EventSchedules where r.EventID == id select r;
				//if (eventSchedules != null)
				//{
				//    foreach (EventSchedule eventSchedule in eventSchedules)
				//    {
				//        _entities.DeleteObject(eventSchedule);
				//        // Delete all the record in LocalRetreatVolunteerJobs Table
				//        var localRetreatVolunteerJobs = from r in _entities.EventVolunteerJobs where r.EventScheduleID == eventSchedule.ID select r;
				//        if (localRetreatVolunteerJobs != null)
				//        {
				//            foreach (EventVolunteerJob temp in localRetreatVolunteerJobs)
				//            {
				//                _entities.DeleteObject(temp);
				//            }
				//        }
				//    }
				//}

                _entities.SaveChanges();
            }
            return View("Deleted");
        }

        public void DeleteEventRegistration(EventRegistration eventRegistration)
        {
            _entities.DeleteObject(eventRegistration);

                  // Delete all the record in LocalRetreatMealBookings Table
            var localRetreatMealBookings = from r in _entities.EventMealBookings where r.EventRegistrationID == eventRegistration.ID select r;
            if (localRetreatMealBookings.Count() > 0)
            {
                foreach (EventMealBooking temp in localRetreatMealBookings)
                {
                    _entities.DeleteObject(temp);
                }
            }
   
            // Delete all the record in LocalRetreatMealBookings Table
            var eventVolunteerJobBookings = from r in _entities.EventVolunteerJobBookings where r.EventRegistrationID == eventRegistration.ID select r;
            RemoveVolunteerJobBooking(eventVolunteerJobBookings);
        }

        private void RemoveVolunteerJobBooking(IQueryable<EventVolunteerJobBooking> pEventVolunteerJobBookings)
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
            }
        }



    }
}
