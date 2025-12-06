using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SMCHSGManager.Controllers
{
    public class EventSignatureController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();
      private int _pageSize = 500;

	  [Authorize]
	  public ActionResult MyEvent(Guid? memberID, DateTime? startDate, DateTime? endDate)   // MyLocalRetreatEvent
      {

		  if (startDate == null)
		  {
			  startDate = new DateTime(2011, 7, 1);
		  }

          if (memberID == null)
          {
              MembershipUser user = Membership.GetUser();
              memberID = ((Guid)(user.ProviderUserKey));
          }

          var eventSignatures = from r in _entities.EventRegistrations
                                where r.MemberInfo.MemberID == memberID &&
                                            r.Event.EventTypeID == 1 && //r.SignTime != null &&
                                            (startDate == null || r.Event.StartDateTime >=startDate.Value) &&
                                              (endDate == null || r.Event.EndDateTime <= endDate.Value)
                                orderby r.Event.StartDateTime
                                select r;

          List<EventRegistrationViewModel> eventRegistrationList = new List<EventRegistrationViewModel>();

          ViewData["LocalRetreatRegistrationCount"] = eventSignatures.Count();

          EventRegistrationController erc = new EventRegistrationController();
          foreach (var eventSignature in eventSignatures)
          {
              int id = eventSignature.ID;

              List<string> vLabels = new List<string>();
              List<string> vValues = new List<string>();
              List<string> mLabels = new List<string>();
              List<string> mValues = new List<string>();

              erc.GetEventVolunteerJobBooking(id, vLabels, vValues, "name");
              erc.GetEventMealBooking(id, mValues, mLabels);
    
              var viewModel = new EventRegistrationViewModel
              {
                  EventRegistration = eventSignature,
                  LocalRetreatMealBookingLabels = mLabels,
                  LocalRetreatMealBookingValues = mValues,
                  EventVolunteerJobBookingLabels = vLabels,
                  EventVolunteerJobBookingValues = vValues,
              };
              eventRegistrationList.Add(viewModel);
          }

          return View(eventRegistrationList);
      }

	  [Authorize(Roles = "Administrator")]
	  public ActionResult EventHistory(DateTime? startDate, DateTime? endDate, int eventTypeID)
	  {
		  List<MemberInfoShortListViewModel> querys = new List<MemberInfoShortListViewModel>();

		  if (eventTypeID == 1)
		  {
			  if (startDate == null)
			  {
				  startDate = new DateTime(2011, 7, 1);
			  }
			  querys = (from r in _entities.EventRegistrations
						where r.Event.EventTypeID == eventTypeID && 
									(startDate == null || r.Event.StartDateTime >= startDate.Value) &&
									(endDate == null || r.Event.EndDateTime <= endDate.Value) //&& r.SignTime != null
						group r by new
						{
							Name = r.MemberInfo.Name,
							IDCardNo = r.MemberInfo.IDCardNo,
							ID = r.MemberID,
						} into result
						select new MemberInfoShortListViewModel()
						{
							ID = result.Key.ID,
							Name = result.Key.Name,
							IDCardNo = result.Key.IDCardNo,
							Count = result.Count(),
						}).ToList();

			  //if (startDate == null)
			  //{
			  //    startDate = (from r in _entities.EventRegistrations where r.Event.EventTypeID == eventTypeID && r.SignTime != null orderby r.Event.StartDateTime select r.Event.StartDateTime).FirstOrDefault();
			  //}
			  //if (endDate == null)
			  //{
			  //    endDate = (from r in _entities.EventRegistrations where r.Event.EventTypeID == eventTypeID && r.SignTime != null orderby r.Event.StartDateTime descending select r.Event.EndDateTime).FirstOrDefault();
			  //}
		  }
		  else
		  {
			  querys = (from r in _entities.GroupMeditationAttendances
						where (startDate == null || r.GroupMeditation.StartDateTime >= startDate.Value) &&
									(endDate == null || r.GroupMeditation.EndDateTime <= endDate.Value)

						group r by new
						{
							Name = r.MemberInfo.Name,
							IDCardNo = r.MemberInfo.IDCardNo,
							ID = r.MemberID,
						} into result
						select new MemberInfoShortListViewModel()
						{
							ID = result.Key.ID,
							Name = result.Key.Name,
							IDCardNo = result.Key.IDCardNo,
							Count = result.Count(),
						}).ToList();

              //if (startDate == null)
              //{
              //    startDate = (from r in _entities.GroupMeditationAttendances orderby r.GroupMeditation.StartDateTime select r.GroupMeditation.StartDateTime).FirstOrDefault();
              //}
              //if (endDate == null)
              //{
              //    endDate = (from r in _entities.GroupMeditationAttendances orderby r.GroupMeditation.StartDateTime descending select r.GroupMeditation.EndDateTime).FirstOrDefault();
              //}
		  }

		  if (startDate == null)
		  {
			  startDate = new DateTime(2011, 1, 1);
		  }
		  if (endDate == null)
		  {
			  endDate = DateTime.Today.ToUniversalTime().AddHours(8);
		  }
		  else
		  {
			  endDate = endDate.Value.AddDays(1).Date;
		  }

		  EventHistoryListViewModel eventRegister = new EventHistoryListViewModel
		  {
			  StartDate = startDate,
			  EndDate = endDate,
			  MemberRegisterList = querys.OrderBy(a => a.Name).ToList(),
		  };

		  ViewData["EventTypeID"] = eventTypeID;
		  ViewData["EventTypeName"] = _entities.EventTypes.Single(a => a.ID == eventTypeID).Name;

		  return View(eventRegister);
	  }


      private static Regex _isNumber = new Regex(@"^\d+$");

      public bool IsInteger(string theValue)
      {
          Match m = _isNumber.Match(theValue);
          return m.Success;
      } //IsInteger


	  [Authorize(Roles = "Administrator")]
	  public ActionResult SignatureList4GMAndLocalRetreat(int? page, string searchContent, int eventTypeID, int? eventID)
        {
            var currentPage = page ?? 1;
            ViewData["PageSize"] = _pageSize;
            ViewData["searchContent"] = searchContent;
            ViewData["EventTypeID"] = eventTypeID;
            
            int memberNO = 0;

            if (!string.IsNullOrEmpty(searchContent) && IsInteger(searchContent))
            {
                memberNO = int.Parse(searchContent);
            }

            DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
            Event currentEvent;
            if (eventID == null)
            {
                DateTime registerBeginTime = now.AddHours(1.5);
				currentEvent = (from r in _entities.Events
								where r.EventTypeID == eventTypeID && registerBeginTime >= r.StartDateTime && now <= r.StartDateTime
								select r).FirstOrDefault();

            }
            else
            {
                currentEvent = _entities.Events.Single(a => a.ID == eventID);
            }

            ViewData["Now"] = String.Format("{0:ddd, d MMM yyyy HH:mm }", now);

            List<MemberInfoShortListViewModel> signatureList = new List<MemberInfoShortListViewModel>();

            if (currentEvent != null)
            {
                if (currentEvent.EventTypeID == 1)
                {
                    var eventRegistrations = from r in _entities.EventRegistrations
                                             where r.EventID == currentEvent.ID && (r.MemberInfo.Name.Contains(searchContent) || searchContent == null || r.MemberInfo.MemberNo == memberNO)
											 //orderby r.MemberInfo.InitiateMemberInfo.MemberTypeID, r.MemberInfo.InitiateMemberInfo.MemberNo
											 orderby r.MemberInfo.Name
                                             select r;

                    foreach (EventRegistration eventRegistation in eventRegistrations)
                    {
                        List<string> mValues1 = new List<string>();
						//decimal personNeedToPay = (decimal)((from r in _entities.EventActivities where r.EventID == eventRegistation.EventID && r.Name.StartsWith("Bless") select r.UnitPrice).FirstOrDefault());
						decimal personNeedToPay = _entities.EventPrices.Single(a=>a.EventID == eventRegistation.EventID && a.EventActivityID == 8).UnitPrice;

                        List<int> tValues = (from r in _entities.EventMealBookings
                                             where r.EventRegistrationID == eventRegistation.ID
                                             select r.EventScheduleID).ToList();

                        foreach (int localRetreatScheduleID in tValues)
                        {
                            int localRetreatActivityID = _entities.EventSchedules.Single(a => a.ID == localRetreatScheduleID).EventActivityID;
							personNeedToPay += (decimal)_entities.EventPrices.Single(a => a.EventActivityID == localRetreatActivityID && a.EventID == eventID).UnitPrice;
                        }

                        MemberInfoShortListViewModel misvm = new MemberInfoShortListViewModel();
                        misvm.ID = eventRegistation.MemberID;
                        misvm.Name = eventRegistation.MemberInfo.Name;
                        misvm.IDCardNo = eventRegistation.MemberInfo.IDCardNo;
                        //misvm.ICOrPassportNo = eventRegistation.MemberInfo.InitiateMemberInfo.ICOrPassportNo;
                        misvm.Money = personNeedToPay;
                        //misvm.MemberType = eventRegistation.MemberInfo.InitiateMemberInfo.MemberType.Name;
                        misvm.MemberNo = eventRegistation.MemberInfo.MemberNo;
						//misvm.MemberFeeExpiredDate = eventRegistation.MemberInfo.InitiateMemberInfo.MemberFeeExpiredDate;
                        signatureList.Add(misvm);
                    }
                    ViewData["Title"] = currentEvent.Title + currentEvent.EventType.Name;

                }
                else if (currentEvent.EventTypeID == 2)  //GM
                {

                    signatureList = (from r in _entities.MemberInfos
                                     where (r.InitiateTypeID == 1 || r.InitiateTypeID == 2) && r.IsActive &&
                                                (r.Name.Contains(searchContent) || searchContent == null || r.MemberNo == memberNO)
									 //orderby r.InitiateMemberInfo.MemberTypeID, r.InitiateMemberInfo.MemberNo
									 orderby  r.MemberNo
                                     select new MemberInfoShortListViewModel()
                                     {
                                         ID = r.MemberID,
                                         Name = r.Name,
                                         IDCardNo = r.IDCardNo,
                                         //ICOrPassportNo = r.InitiateMemberInfo.ICOrPassportNo,
                                         //MemberType = r.InitiateMemberInfo.MemberType.Name,
                                         MemberNo = r.MemberNo,
                                         //MemberFeeExpiredDate = r.InitiateMemberInfo.MemberFeeExpiredDate,
                                     }).ToList();

                    ViewData["Title"] = String.Format("{0:ddd, d MMM yyyy  }", currentEvent.StartDateTime) + String.Format("{0:HH:mm}", currentEvent.StartDateTime) + " to " + String.Format("{0:HH:mm}", currentEvent.EndDateTime) + ' ' + currentEvent.EventType.Name;

                }

                ViewData["eventSigedMemberIDs"] = (from r in _entities.EventRegistrations where r.EventID == currentEvent.ID && r.SignTime != null select r.MemberID).ToList();
                //List<MemberInfoShortListViewModel> signatureListNoSign = new List<MemberInfoShortListViewModel>();
                //foreach (var item in signatureList)
                //{
                //    var eventSignatures = from r in _entities.EventRegistrations where r.EventID == currentEvent.ID && r.MemberID == item.ID && r.SignTime != null select r;
                //    if (eventSignatures.Count() == 0)
                //    {
                //        signatureListNoSign.Add(item);
                //    }
                //}

                //ViewData["TotalPages"] = (int)Math.Ceiling((float)signatureListNoSign.Count() / _pageSize); 
                ViewData["TotalPages"] = (int)Math.Ceiling((float)signatureList.Count() / _pageSize);

                if ((int)ViewData["TotalPages"] < currentPage)
                {
                    currentPage = 1;
                }
                ViewData["CurrentPage"] = currentPage;

                //signatureList = (signatureListNoSign.AsQueryable().Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
                signatureList = (signatureList.AsQueryable().Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

                //ViewData["Title"] = currentEvent.Title;
                ViewData["EventID"] = currentEvent.ID;

            }

            return View(signatureList);
        }


	  [Authorize(Roles = "Administrator")]
	  public ActionResult SignatureList(int eventTypeID, string searchContent)
        {
            DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
            DateTime nowMore = now.AddHours(1.5);
            List<Event> currentEvents = (from r in _entities.Events
                                         where r.EventTypeID == eventTypeID && nowMore >= r.StartDateTime && now <= r.StartDateTime
                                         select r).ToList();

            List<Event> allEventList = new List<Event>();
            List<List<MemberInfoShortListViewModel>> allSginatureList = new List<List<MemberInfoShortListViewModel>>();
            foreach (Event aEvent in currentEvents)
            {
                List<MemberInfoShortListViewModel> signatureList = new List<MemberInfoShortListViewModel>();

                if (aEvent.EventTypeID == 3) // GM convienent method GM
                {
                    signatureList = (from r in _entities.MemberInfos
                                     where r.InitiateTypeID == 3 && (r.Name.Contains(searchContent) || searchContent == null)
                                     orderby r.Name
                                     select new MemberInfoShortListViewModel()
                                     {
                                         ID = r.MemberID,
                                         Name = r.Name,
                                     }).ToList();

                }
                else
                {
                    var eventRegistrations = from r in _entities.EventRegistrations
                                             where r.EventID == aEvent.ID && (r.MemberInfo.Name.Contains(searchContent) || searchContent == null)
                                             orderby r.MemberInfo.Name
                                             select r;

                    foreach (EventRegistration eventRegistation in eventRegistrations)
                    {
                        List<string> mValues1 = new List<string>();

                        MemberInfoShortListViewModel misvm = new MemberInfoShortListViewModel();
                        misvm.ID = eventRegistation.MemberID;
                        misvm.Name = eventRegistation.MemberInfo.Name;
                        signatureList.Add(misvm);
                    }

                }
                allEventList.Add(aEvent);

                List<MemberInfoShortListViewModel> signatureListNoSign = new List<MemberInfoShortListViewModel>();
                foreach (var item in signatureList)
                {
                    var eventSignatures = from r in _entities.EventRegistrations where r.EventID == aEvent.ID && r.MemberID == item.ID && r.SignTime != null select r;
                    if (eventSignatures.Count() == 0)
                    {
                        signatureListNoSign.Add(item);
                    }
                }
                allSginatureList.Add(signatureListNoSign);
            }

            ViewData["EventTypeID"] = allEventList;

            return View(allSginatureList);
        }

		//public ActionResult Signature(Guid ID, int eventID)
		//{
		//    var viewModel = _entities.Events.Single(a => a.ID == eventID);
		//    viewModel.OrganizerNameID = ID;              // temp use

		//    return View(viewModel);
		//}

		//[Authorize, HttpPost]
		//public ActionResult Signatured(Guid ID, int eventID)
		//{

		//    var eventRegistrations = from r in _entities.EventRegistrations where r.EventID == eventID && r.MemberID == ID select r;
		//    if (eventRegistrations.Count() > 0)
		//    {
		//        EventRegistration eventRegistration = eventRegistrations.FirstOrDefault();
		//        eventRegistration.SignTime = DateTime.Now.ToUniversalTime().AddHours(8);
		//        UpdateModel(eventRegistration, "EventRegistration");
		//    }
		//    else
		//    {
		//        EventRegistration eventRegistration = new EventRegistration();
		//        eventRegistration.MemberID = ID;
		//        eventRegistration.EventID = eventID;
		//        eventRegistration.SignTime = DateTime.Now.ToUniversalTime().AddHours(8);
		//        _entities.AddToEventRegistrations(eventRegistration);
		//    }
		//    _entities.SaveChanges();

		//    string msg = "Thank you for your signing your attendance.";
		//    return Content(msg);
		//}

		//public ActionResult UnSign(Guid ID, int eventID)
		//{
		//     var viewModel = _entities.Events.Single(a => a.ID == eventID);
		//    viewModel.OrganizerNameID = ID;              // temp use

		//    return View(viewModel);
		//}

		//[Authorize, HttpPost]
		//public ActionResult UnSigned(Guid ID, int eventID)
		//{
		//    var eventRegistration = (from r in _entities.EventRegistrations where r.EventID == eventID && r.MemberID == ID select r).FirstOrDefault();
		//    int eventTypeID = eventRegistration.Event.EventTypeID;

		//    if (eventTypeID == 2 || eventTypeID == 3)
		//    {
		//        _entities.DeleteObject(eventRegistration);
		//    }
		//    else
		//    {
		//        eventRegistration.SignTime = null;
		//        UpdateModel(eventRegistration, "EventRegistration");
		//    }
		//    _entities.SaveChanges();

		//    string msg = "Thank you.";
		//    return Content(msg);
		// }
        
        ////
        //// GET: /EventSignature/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        ////
        //// GET: /EventSignature/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

     
    
    }
}
