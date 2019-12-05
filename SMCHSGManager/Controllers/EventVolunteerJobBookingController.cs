using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;

namespace SMCHSGManager.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class EventVolunteerJobBookingController : Controller
    {
        //
        // GET: /LocalRetreatVolunteerJobBookings/

		private SMCHDBEntities _entities = new SMCHDBEntities();
		[Authorize(Roles = "Administrator")]
		public ActionResult Table(int localRetreatID)
        {
            //return View(_entities.EventVolunteerJobBookings);

            ViewData["LocalRetreatID"] = localRetreatID;

            List<EventVolunteerJobBooking> localRetreatVolunteerJobBookings = (from r in _entities.EventVolunteerJobBookings
                            where r.EventRegistration.EventID == localRetreatID
                            orderby r.EventVolunteerJob.VolunteerJobTypeID, r.EventVolunteerJob.EventSchedule.DateTimeFrom
                            select r).ToList();

            List<string> volunteerJobNames = new List<string>();
            List<string> volunteerJobTimes = new List<string>();
            List<string> PersonInCharges = new List<string>();
            EventRegistrationController erc = new EventRegistrationController();
            erc.RetrieveEventVolunteerJobBooking(localRetreatVolunteerJobBookings, volunteerJobNames, volunteerJobTimes, "Remark", PersonInCharges);

            List<EventVolunteerJobBookingViewModel> viewModelList = new List<EventVolunteerJobBookingViewModel> ();
            EventVolunteerJobBookingViewModel viewModel;
            for (int i = 0; i < volunteerJobNames.Count; i++ )
            {
                viewModel = new EventVolunteerJobBookingViewModel();
                viewModel.VolunteerJobName = volunteerJobNames[i];
                viewModel.VolunteerJobTime = volunteerJobTimes[i];
                viewModel.PersonInCharge = PersonInCharges[i];
                viewModelList.Add(viewModel);
            }

			ViewData["LocalRetreat"] = _entities.Events.Single(a => a.ID == localRetreatID);
			ViewData["VolunteerJobTypes"] = _entities.VolunteerJobTypes.ToList();

            if (viewModelList.Count() > 0)
            {
                return View(viewModelList);
            }
            else
            {
                ViewData["Message"] = "There is no local retreat Volunteer Job Booked record in database.";
                return View();
            }

        }

        //
        // GET: /LocalRetreatVolunteerJobBookings/Details/5

		public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LocalRetreatVolunteerJobBookings/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /LocalRetreatVolunteerJobBookings/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /LocalRetreatVolunteerJobBookings/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /LocalRetreatVolunteerJobBookings/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /LocalRetreatVolunteerJobBookings/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LocalRetreatVolunteerJobBookings/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
