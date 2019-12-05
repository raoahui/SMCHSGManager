using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Diagnostics;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Text;
using System.Reflection;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SMCHSGManager.Helper
{
	public class EventEmailJob : IJob
	{
		private String eventName;
		private DateTime eventStartTime;
		private DateTime eventEndTime;
		private DateTime regStartTime;
		private DateTime regEndTime; 

		private SMCHDBEntities _entities = new SMCHDBEntities();

		#region IJob Members

		public void Execute(IJobExecutionContext context)
		{
			try
			{
				JobDataMap dataMap = context.MergedJobDataMap;

				eventName = dataMap.GetString("eventName");
				eventStartTime = dataMap.GetDateTime("eventStartTime");
				eventEndTime = dataMap.GetDateTime("eventEndTime");
				regStartTime = dataMap.GetDateTime("regStartTime");
				regEndTime = dataMap.GetDateTime("regEndTime");

				EmailMessage em = new EmailMessage();

				em.Subject = eventName + " Local Retreat";
				em.Message = "Dear Saints, \r\n\r\n" + "Local Retreat – " + string.Format("{0: ddd, d-MMM-yyyy.}",  eventStartTime);
				em.Message += "\r\n\r\nA local retreat that coincides with " + eventName + " is being organized.";
				em.Message += "\r\n\r\nPlease register at SMCHA(S) website http://www.smchsg.com/";
				em.Message += "\r\nAll initiates are encouraged to register for local retreats through this website to reduce administration work.";
 
				em.Message += "\r\n\r\nClosing date for sign-up is " + string.Format("{0: ddd, d-MMM-yyyy.}",  regEndTime) + "before 12:00 midnight, to facilitate preparations.";

				em.Message += "\r\n\r\nAll participants are to refer to the attached <<Local Retreats_Guidelines For Application + Rules & Regulations.pdf>>.";
				em.Message += "\r\nAll participants are reminded to bring their own eating utensils.";

				//em.Message += "\r\n\r\nPlease take note that there is still group meditation on Saturday evening as the local retreat ends at 3:00p.m. on Saturday afternoon.
				em.Message += "\r\n\r\nWe look forward to your participation.";

//Attachment: Local Retreats_Guildelines for Application + Rules & Regulations (RevB).pdf
				em.Message += "\r\n\r\n\r\n\r\n\r\nWishing you Master's Love & Blessings,\r\nSingapore Centre";


				em.From = "admin@smchsg.com";
				//em.To = "ftmnwl64@singnet.com.sg, chinghai@singnet.com.sg";
				em.To = "chinghai@singnet.com.sg";

				List < SMCHSGManager.Models.MemberInfo > mis = _entities.MemberInfos.Where(a => a.InitiateTypeID == 1).OrderBy(a => a.aspnet_Users.UserName).ToList();
				foreach (SMCHSGManager.Models.MemberInfo mi in mis)
				{
					string userName = mi.aspnet_Users.UserName;
					MembershipUser user = Membership.GetUser(userName);
					if (!string.IsNullOrEmpty(user.Email) && user.Email.Trim() != "password@smchsg.com")
					{
						if (string.IsNullOrEmpty(em.bcc))
						{
							em.bcc = user.Email;
						}
						else
						{
							string email = user.Email.Replace('/', ',');
							em.bcc += ", " + user.Email;
						}
					}
				}
				//em.bcc = "hiseekersm@gmail.com";
				em.AttachFilePath = "";

				EmailService es = new EmailService();
				es.SendMessage(em);

				Debug.WriteLine("Scheduler is a success :" + DateTime.Now.ToString());

			}
			catch (JobExecutionException )
			{
				Debug.WriteLine("Scheduler Job Exception");
			}

		}

		#endregion
	}
}