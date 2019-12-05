using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;

namespace SMCHSGManager.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EventVolunteerJobController : Controller
    {
        //
        // GET: /EventVolunteerJob/
        SMCHDBEntities _entities = new SMCHDBEntities();

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int localRetreatID)
        {
            ViewData["LocalRetreatID"] = localRetreatID;

            var viewModel = from r in _entities.EventVolunteerJobs
                            where r.EventSchedule.Event.ID == localRetreatID
                            orderby r.VolunteerJobTypeID, r.EventSchedule.DateTimeFrom
                            select r;

            if (viewModel.Count() > 0)
            {
                return View(viewModel);
            }
            else
            {
                ViewData["Message"] = "There is no local retreat Volunteer Job in database.";
                return View();
            }
        }

        //
        // GET: /EventVolunteerJob/Details/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Details(int id)
        {
            return View();
        }

   }
}
