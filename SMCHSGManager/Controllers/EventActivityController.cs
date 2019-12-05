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
    //[Authorize(Roles = "Administrator")]
    public class EventActivityController : Controller
    {

        private SMCHDBEntities _entities = new SMCHDBEntities();
        //
        // GET: /LocalRetreatMealBookings/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index()
        {
			return View(_entities.EventActivities.ToList());
        }

		////
		//// GET: /LocalRetreatMealBookings/Details/5

		//public ActionResult Details(int id)
		//{
		//    return View();
		//}

        //
        // GET: /LocalRetreatMealBookings/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /LocalRetreatMealBookings/Create

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
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
        // GET: /LocalRetreatMealBookings/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
            var viewModel = _entities.EventActivities.Single(a => a.ID == id);

            return View(viewModel);
        }

        //
        // POST: /LocalRetreatMealBookings/Edit/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection)
        {
            var eventActivity = _entities.EventActivities.Single(a => a.ID == id);
            try
            {
                //eventActivity.Name = collection.Get("Name");
                eventActivity.Remark = collection.Get("Remark");

                UpdateModel(eventActivity, "EventActivity");
                _entities.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }


            ////ensure that the model is valid and return the errors back to the view if not.
            //if (!ModelState.IsValid)
            //    return View(newEventActivity);

            ////locate the customer in the database and update the model with the views model.
            //EventActivity thisEventActivity = _entities.EventActivities.Single(a => a.ID == id);
            //newEventActivity.EventID = thisEventActivity.EventID;
            //if (TryUpdateModel<EventActivity>(thisEventActivity))
            //    _entities.SaveChanges();
            //else
            //    return View(newEventActivity);

            ////return to the index page if complete
            //return RedirectToAction("index");

        }

        //
        // GET: /LocalRetreatMealBookings/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LocalRetreatMealBookings/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
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
