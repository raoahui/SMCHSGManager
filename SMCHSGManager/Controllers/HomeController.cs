using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace SMCHSGManager.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        
        private SMCHDBEntities _entities = new SMCHDBEntities();
        //
        // GET: /Home/

		public void SwitchSMCHDbUserInServiceLayer()
		{
			// Get connection string and set some parameters
			SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(((EntityConnection)_entities.Connection).StoreConnection.ConnectionString);
			sb.Password = "iLoveMaster";

			// Set new connection string to EF
			((EntityConnection)_entities.Connection).StoreConnection.ConnectionString = sb.ConnectionString;
		}

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to SupremeMaster Ching Hai Singapore Association!";

            System.Globalization.CultureInfo info = new System.Globalization.CultureInfo("en-US", false);
            System.Globalization.Calendar calendar = info.Calendar;
            DateTime lastDay = new System.DateTime(2013, 1, 24, calendar);

            DateTime lToday = DateTime.Now.ToUniversalTime().AddHours(8);
            // For debug in local, need to use DateTime.Now;

            ViewData["LeftDays"] = (lastDay - lToday).Days;

            bool initiateOnly = User.IsInRole("Initiate");
            
            EventController ec = new EventController();
            var upcomingEvents = ec.GetUpcomingEvents(true, initiateOnly);
			int eventNo = 1;
			if (upcomingEvents.Count() < eventNo)
			{
				eventNo = upcomingEvents.Count();
			}

            AnnouncementController ac = new AnnouncementController();
            var announcements = ac.GetAnnouncements(null, initiateOnly, null);
			int announcementNo = 1;
			if (announcements.Count() < announcementNo)
			{
				announcementNo = announcements.Count();
			}

            DateTime checkInTimeStart = lToday.AddMinutes(90 + 15);
            DateTime checkInTimeEnd = lToday.AddMinutes(-15);
            var groupMeditation = _entities.GroupMeditations.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTimeEnd && a.InitiateTypeID == 1).FirstOrDefault();
            var groupMeditationAttendances = new List<GroupMeditationAttendance>();
            if (groupMeditation != null) {
                groupMeditationAttendances = (from r in _entities.GroupMeditationAttendances
                                              where r.GroupMeditationID == groupMeditation.ID && r.CheckInTime != null
                             orderby r.CheckInTime
                             select r).ToList();
            }
            
            var viewModel = new HomeViewModel
            {
				Announcements = announcements.Take(announcementNo).ToList(),
				UpcomingEvents = upcomingEvents.Take(eventNo).ToList(),
				AnnouncementImages = ac.GetAnnouncementImage(announcements.Take(announcementNo).ToList()),
                GroupMeditation = groupMeditation,
                GroupMeditationAttendances = groupMeditationAttendances,
            };

            return View(viewModel);
        }

		[Authorize(Roles = "SuperAdmin")]
		public ActionResult SendAllEmail()
        {
            return View();
        }

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "SuperAdmin")]
		public ActionResult SendAllEmail(FormCollection collect)
        {
            List<string> testGroup = new List<string>();
			//testGroup.Add("Rao Hui");
			//testGroup.Add("Li Rao");
			//testGroup.Add("Ng Wee Ling");
			//testGroup.Add("Shung Chook Ying");
			//testGroup.Add("Wong Yat Sun");
			//testGroup.Add("Santi Pradjinata");
			//testGroup.Add("Tay Chin Joo");
			//testGroup.Add("Dong Miao Bo");
			//testGroup.Add("Ma Ze Yuan");        // g
			//testGroup.Add("Chong Mui Heng"); // g
			//testGroup.Add("Wong Yue Kah"); // g

             //testGroup.Add("Siet Tiat Hwa");
            //testGroup.Add("Yap Lay Ching");

			//MembershipUserCollection muc = Membership.GetAllUsers();
			//foreach (MembershipUser user in muc)
			//{
			//        if(//if (Roles.IsUserInRole(user.UserName, "Administrator"))
			//        {
			//            sendUserEmail(user);
			//        }
			//}

			//SwitchSMTVDbUserInServiceLayer();

			List<MemberInfo> mis = _entities.MemberInfos.Where(a => a.InitiateTypeID == 1).OrderBy(a => a.aspnet_Users.UserName).ToList();
			foreach(MemberInfo mi in mis)
			{
				string userName = mi.aspnet_Users.UserName;
				MembershipUser user = Membership.GetUser(userName);
				//if ( user.UserName.ToLower() == "zhong wen ju")
				//if (user.UserName.ToLower() == "ho chai yan")
				//if (user.UserName.ToLower() == "liu chune")  // not valid email
				//if (user.UserName.ToLower() == "tay kheng song")
				//if (user.UserName.ToLower() == "tan huan guang")
				//if (user.UserName.ToLower() == "sarah yong") 
				//if (user.UserName.ToLower() == "wang xin")
				//if (user.UserName.ToLower() == "ong liou kee")
				//if (user.UserName.ToLower() == "mun chaing lu")
				//if (user.UserName.ToLower() == "zheng jing")
				//if (user.UserName.ToLower() == "lum choy yin") 
				//if (user.UserName.ToLower() == "ong sai jar" || user.UserName.ToLower() == "sim chin chye")
				//if (user.UserName.ToLower() == "sim chin chye")
				//if (user.UserName.ToLower() == "goh yu huat")
				//if (user.UserName.ToLower() == "tee bee yan")
				//if (user.UserName.ToLower() == "khoo hsien hui")
				//if (user.UserName.ToLower() == "om kimnowark")
				//if (user.UserName.ToLower() == "su qianning")
				//if (!user.Email.ToLower().EndsWith("@smchsg.com") || user.UserName.ToLower() == "rao hui")
				//if (user.UserName.ToLower() == "tan twee nong")
				//if (user.UserName.ToLower() == "harpandy tandadjaja")
				//if (user.UserName.ToLower() == "teo gim seng" || user.UserName.ToLower() == "yeo yong mui" || user.UserName.ToLower() == "teo kai ren")
				//if (user.UserName.ToLower() == "lim choy lin")
				//if (user.UserName.ToLower() == "tong qing wen")
				//if (user.UserName.ToLower() == "lee hui ching")
				//if (user.UserName.ToLower() == "lum choy yin") 
				//if (user.UserName.ToLower() == "tham mei fun")
				//if (user.UserName.ToLower() == "sohn eun soo")
				//if (user.UserName.ToLower() == "poh sok hiang")
				//if ()
				//if (user.UserName.ToLower() == "chung nyet lee" || user.UserName.ToLower() == "aden gan" || user.UserName.ToLower() == "peggy ong")
				//if (user.UserName.ToLower() == "hoang hai thai duong" || user.UserName.ToLower() == "doan thi ni ni" )
				//if (user.UserName.ToLower() == "liew yuen thian")
				//if (user.UserName.ToLower() == "chua kok cheng" || user.UserName.ToLower() == "sheng li hong" || user.UserName.ToLower() == "low chip khoon")
				//if (user.UserName.ToLower() == "low beng teng" || user.UserName.ToLower() == "zhao zhe ming")
				//if (user.UserName.ToLower() == "nguyen thi y nhi")
				//if (user.UserName.ToLower() == "lim ji yuen gloria") 
				//if (user.UserName.ToLower() == "lim tiong ghor")
				//if (user.UserName.ToLower() == "lim kim yuan nicholas")
				//if (user.UserName.ToLower() == "jeffrey peh" || user.UserName.ToLower() == "sng chor meng" || user.UserName.ToLower() == "wong soon yum")
				//if (user.UserName.ToLower() == "heng yong chai")   
				//if (user != null && user.UserName.ToLower() == "yu binbing")
				//if (user != null && user.UserName.ToLower() == "leow boon que")
				//if (user != null && user.UserName.ToLower() == "wang yi")
				if (user != null && user.UserName.ToLower() == "eu gek li")
				{
					sendUserEmail(user);
				}
				//"heng swee choo"  "Namsrai Uyanga" 
			}
			

			//foreach (string name in testGroup)
			//{
			//    MembershipUser user = Membership.GetUser(name);
			//    sendUserEmail(user);
			//}

            string msg = "Done!";
            return Content(msg);
        }

        public void sendUserEmail(MembershipUser user)
        {
            string randomNewPW = "chinghai"; // user.ResetPassword();

            EmailMessage em = new EmailMessage();
            em.Subject = "Your User Name and Password for the Supreme Master Ching Hai Association (Singapore) Website";
			string fullUrl = "http://www.smchsg.com/Account/logon"; // urlBase + verifyUrl;


//            Hallo Saints,

//Welcome to a new website for Supreme Master Ching Hai Association (Singapore)!
//http://www.smchsg.com/

//Your login information is shown below.
//            If you have any problem or doubt, please:
//1) Use the Help file http://www.smchsg.com/Help or
//2) Send an e-mail to admin@smchsg.com

//With Master’s Love,
//Admin

           
            em.Message = "Hallo Saints,\r\n" + "\r\nWelcome to a new website for Supreme Master Ching Hai Association (Singapore)! \r\n http://www.smchsg.com/\r\n" + "\r\nYour login information is shown below." + "\r\nUser Name : " + user.UserName + "\r\nPassword : ";
            if (Roles.IsUserInRole(user.UserName, "Administrator"))
            {
                randomNewPW = "**********";
            }

			EventController ec = new EventController();
			var upcomingEvents = ec.GetUpcomingEvents(true, true).Take(1);

            em.Message += randomNewPW + "\r\nPlease login at " + fullUrl + "\r\nFor security reasons, you are advised to change your password immediately after your first login.\r\n";
			//em.Message += "\r\nYou can then proceed to register for a coming local retreat on " + string.Format("{0:ddd, d MMM yyyy HH:mm }", upcomingEvents.FirstOrDefault().StartDateTime.AddMinutes(90)) + ". You need to do this by " + string.Format("{0:ddd, d MMM yyyy}", upcomingEvents.FirstOrDefault().RegistrationCloseDate) + "\r\n"; // " Tuesday, 9-Aug-2011.\r\n";
			//em.Message += "\r\nKindly update your information under \"My Account\" " + "WITHIN THE NEXT TWO WEEKS."+ " Thereafter, changes in any members' information can only be done through filling out an update form at Singapore Centre. Information given is kept confidential.\r\n";
			em.Message += "\r\nIf you have any problem or doubt, please: \r\n";
			em.Message += "    1) Use the Help file http://www.smchsg.com/Help or\r\n";
			em.Message += "    2) Send an e-mail to admin@smchsg.com\r\n\r\n";

			em.Message += "With Master' s Love,\r\nAdmin";

			em.Message += "\r\n\r\n\r\n\r\n\r\nNOTICE: If you are not the intended recipient, please notify the sender and delete the message and any other record of it from your system immediately.  This message is intended for use of the party to whom it is addressed.  Information is privileged & confidential.  You should not use, copy, disseminate or distribute it for any purpose or disclose its contents to any other party (person or entity).  Unauthorized use, disclosure or reproduction is prohibited & unlawful.  We do not guarantee the integrity of any e-mails or attached files and are not responsible for any changes made to them by any other person."; 


			//em.Message += "\r\nIf you have any problem or doubt, please send email to admin@smchsg.com\r\n" + "\r\nWML\r\nAdmin"; 
           
            em.From = "admin@smchsg.com";
            em.To = user.Email.Trim();

            EmailService es = new EmailService();
            es.SendMessage(em);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Links()
        {
            return View();
        }

      }
}
