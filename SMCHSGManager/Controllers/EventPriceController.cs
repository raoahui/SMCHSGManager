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
    public class EventPriceController : Controller
    {

        private SMCHDBEntities _entities = new SMCHDBEntities();
        //
        // GET: /LocalRetreatMealBookings/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int eventID)
        {
            var viewModel = from r in _entities.EventPrices where r.EventID == eventID select r;

            return View(viewModel);
        }

        //
        // GET: /LocalRetreatMealBookings/Details/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Details(int id)
        {
            return View();
        }
        
        //
        // GET: /LocalRetreatMealBookings/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int eventID, int eventActivityID)
        {
			var viewModel = _entities.EventPrices.Single(a => a.EventID == eventID && a.EventActivityID == eventActivityID);

            return View(viewModel);
        }

        //
        // POST: /LocalRetreatMealBookings/Edit/5

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int eventID, int eventActivityID, FormCollection collection)
        {
			var eventActivity = _entities.EventPrices.Single(a => a.EventID == eventID && a.EventActivityID == eventActivityID);
            try
            {
                eventActivity.UnitPrice = decimal.Parse(collection.Get("UnitPrice"));

				UpdateModel(eventActivity, "EventPrice");
                _entities.SaveChanges();

                return RedirectToAction("Index", new { eventID = eventActivity.EventID });
            }
            catch
            {
                return View();
            }

        }

	
    }
}
