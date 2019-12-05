using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.ViewModel;
using SMCHSGManager.Models;

namespace SMCHSGManager.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class EventMealBookingController : Controller
    {
        //
        // GET: /EventMealBooking/

		private SMCHDBEntities _entities = new SMCHDBEntities();

		[Authorize(Roles = "Administrator")]
		public ActionResult Table(int localRetreatID)
        {

            string localRetreatName = _entities.Events.Single(a => a.ID == localRetreatID).Title;
            ViewData["localRetreatName"] = localRetreatName;
            ViewData["LocalRetreatID"] = localRetreatID;
             
            var query3 = from localRetreatMealBookings in _entities.EventMealBookings
                         where localRetreatMealBookings.EventRegistration.EventID == localRetreatID
                         group localRetreatMealBookings by localRetreatMealBookings.EventScheduleID into result
                         select new
                         {
                             EventScheduleID = result.Key,
                             Count = result.Count()
                         };

             List<EventMealBookingViewModel> viewModelList = new List<EventMealBookingViewModel>();
             EventMealBookingViewModel viewModel;
             foreach (var q in query3)
             {
                 var localRertreatSchedule = _entities.EventSchedules.Single(a => a.ID == q.EventScheduleID);
				 decimal unitPrice = _entities.EventPrices.Single(a => a.EventID == localRetreatID && a.EventActivityID == localRertreatSchedule.EventActivityID).UnitPrice;
                 string mealName = String.Format("{0:d}", localRertreatSchedule.DateTimeFrom);
                 mealName += ' ' + localRertreatSchedule.EventActivity.Name;

                 viewModel = new EventMealBookingViewModel();
                 viewModel.MealNameDate = mealName;
                 viewModel.Count = q.Count;
                 viewModel.UnitPrce = unitPrice;
                 viewModel.EventScheduleID = q.EventScheduleID;
                 viewModelList.Add(viewModel);
             }

             viewModel = new EventMealBookingViewModel();
             EventPrice eventPrice = _entities.EventPrices.Single(a=>a.EventID == localRetreatID && a.EventActivityID == 8); // Blessing Food
  
			 viewModel.MealNameDate = eventPrice.EventActivity.Name;
             viewModel.Count = (from r in _entities.EventRegistrations where r.EventID == localRetreatID select r).Count();
			 viewModel.UnitPrce = (decimal)eventPrice.UnitPrice;

             var localRetreatScheduleID = from r in _entities.EventSchedules
                                          where r.EventID == localRetreatID && r.EventActivity.Name.StartsWith("Bless") && r.EventID == localRetreatID
                                          select r.ID;

             viewModel.EventScheduleID = localRetreatScheduleID.FirstOrDefault();

             //viewModel.EventScheduleID = 
             //    //_entities.EventSchedules.Single(a => a.EventActivityID == eventActivity.ID).ID;

             viewModelList.Add(viewModel);

             return View(viewModelList);

        }

        //
        // GET: /EventMealBooking/Details/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Details(int localRetreatScheduleID)
        {
            EventSchedule localRetreatSchedule = _entities.EventSchedules.Single(a => a.ID == localRetreatScheduleID);
            ViewData["LocalRetreatTitle"] = localRetreatSchedule.Event.Title;
           
            ViewData["MealName"]  = String.Format("{0:d}", localRetreatSchedule.DateTimeFrom) + ' ' + localRetreatSchedule.EventActivity.Name;
            ViewData["LocalRetreatID"] = localRetreatSchedule.EventID;
 
            var query3 = from localRetreatMealBookings in _entities.EventMealBookings
                         where localRetreatMealBookings.EventScheduleID == localRetreatScheduleID
                         select localRetreatMealBookings.EventRegistration.MemberInfo.Name;

            return View(query3);
        }

        
    }
}
