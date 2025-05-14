using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMCHSGManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.ViewModel
{
    public class AnnouncementViewModel
    {
        public Announcement Announcement { get; set; }
        public List<int> AnnouncementIDs { get; set; }
		//public List<UploadFile> UploadFiles { get; set; }
     }

     public class EventViewModel
    {
        public Event Event { get; set; }
        public List<Location> Locations { get; set; }
        public List<EventType> EventTypes { get; set; }
        public List<int> EventIDs { get; set; }
        public List<SelectListItem> scheduleModelSelectLists { get; set; }
     }

    public class EventListViewModel
    {
        public List<Event> UpcomingEvents { get; set; }
        public List<Event> RecentEvents { get; set; }
		public string[] UpcomingEventsImages { get; set; }
		public string[] RecentEventsImages { get; set; }
	}

    public class LocationViewModel
    {
        public Location Location { get; set; }
        public List<int> LocationIDs { get; set; }
    }

    public class HomeViewModel
    {
        public List<Announcement> Announcements { get; set; }
        public List<Event> UpcomingEvents { get; set; }
        public string[] AnnouncementImages { get; set; }
        public GroupMeditation GroupMeditation { get; set; }
        public List<GroupMeditationAttendance> GroupMeditationAttendances { get; set; }
    }

	public class InitiateVisitorViewModel
	{
		public InitiateVisitor InitiateVisitor { get; set; }
		public List<Gender> Genders { get; set; }
	}

	public class GMVolunteerJobNameViewdModel
	{
		public GMVolunteerJobName GMVolunteerJobName { get; set; }
		public List<MemberInfo> MemberInfo { get; set; }
	}

}