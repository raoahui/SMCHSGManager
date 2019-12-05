using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMCHSGManager.Models;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMCHSGManager.ViewModel
{
 
    public class EventRegistrationViewModel
    {
        public EventRegistration EventRegistration { get; set; }
        public List<String> LocalRetreatMealBookingLabels { get; set; }
        public List<String> LocalRetreatMealBookingValues { get; set; }
        public List<String> EventVolunteerJobBookingLabels { get; set; }
        public List<String> EventVolunteerJobBookingValues { get; set; }
        public List<String> EventBreakDateTimeLabels { get; set; }
        public List<String> EventBreakDateTimeValues { get; set; }
        //public List<SelectListItem> LocalRetreatBreakDateTimeSelectLists { get; set; }
        public List<MemberInfo> MemberInfo { get; set; }
    }

    public class EventScheduleViewModel
    {
        public EventSchedule EventSchedule { get; set; }
        public List<EventActivity> EventActivity { get; set; }
        public List<ScheduleOffset> ScheduleOffset { get; set; }
        public List<String> VolunteerJobNameLabels { get; set; }
        public List<String> VolunteerJobNameValues { get; set; }
    }

    public class LocalRetreatScheduleTemplateViewModel
    {
        public LocalRetreatScheduleTemplate LocalRetreatScheduleTemplate { get; set; }
        public List<EventActivity> EventActivity { get; set; }
        public List<ScheduleOffset> ScheduleOffset { get; set; }
    }

    public class EventMealBookingViewModel
    {
        public int EventScheduleID { get; set; } 
        public String MealNameDate { get; set; }
        public int Count { get; set; }
        public Decimal UnitPrce { get; set; }
    }

    public class EventVolunteerJobBookingViewModel
    {
        public int EventScheduleID { get; set; }
        public String VolunteerJobName { get; set; }
        public String VolunteerJobTime { get; set; }
        public String PersonInCharge { get; set; }
    }


    public class EventHistoryListViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
        public DateTime? EndDate { get; set; }

        public List<MemberInfoShortListViewModel> MemberRegisterList { get; set; }
    }

   
}
