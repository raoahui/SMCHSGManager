using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using SMCHSGManager.Helper;
using SMCHSGManager.Models;
using Quartz.Core;
using System.Web.Security;
using System.IO;

namespace SMCHSGManager.Helper
{
	public class SchedulerHelper
	{
		static IScheduler sched;
		// construct a scheduler factory
		public static void InitializeFactory()
		{
			ISchedulerFactory schedFact = new StdSchedulerFactory();

			// get a scheduler
			sched = schedFact.GetScheduler();

			//PenangScheduler();
			EventNotificationScheduler();
			sched.Start();
		}

		private static void PenangScheduler()
		{
			// construct job
			JobDetailImpl penangJobDetail = new JobDetailImpl("PenangApplicationJ", typeof(PenangAshramJob));

			// Set to fire x minutes/days/etc
			CronTriggerImpl penangTrigger = new CronTriggerImpl("PenangApplicationT");

			CronExpression cron = new CronExpression("0 0 0 ? * MON,THU");
			cron.TimeZone = TimeZoneInfo.CreateCustomTimeZone("Singapore", new TimeSpan(8, 0, 0), "Singapore", "Singapore");
			penangTrigger.CronExpression = cron;

			// Set start time
			penangTrigger.StartTimeUtc = PenangAshramJob.JobStartTime;

			sched.ScheduleJob(penangJobDetail, penangTrigger);
		}

		private static void EventNotificationScheduler()
		{
			SMCHDBEntities _entities = new SMCHDBEntities();

			//Get list of dates
			DateTime today = DateTime.Today;
			String jobName = "EventNotificationJob";
			List<Event> eventList = _entities.Events.Where(a => a.RegistrationOpenDate >= today).ToList();
			JobDetailImpl eventJobDetail = new JobDetailImpl(jobName, typeof(EventEmailJob));

			sched.AddJob(eventJobDetail, true);

			foreach (Event eventEntry in eventList)
			{
				SimpleTriggerImpl trigger = new SimpleTriggerImpl(eventEntry.Title + "_EventTrigger", eventEntry.RegistrationOpenDate.ToUniversalTime());

				trigger.JobDataMap["eventName"] = eventEntry.Title;
				trigger.JobDataMap["eventStartTime"] = eventEntry.StartDateTime;
				trigger.JobDataMap["eventEndTime"] = eventEntry.EndDateTime;
				trigger.JobDataMap["regStartTime"] = eventEntry.RegistrationOpenDate;
				trigger.JobDataMap["regEndTime"] = eventEntry.RegistrationCloseDate;

				trigger.JobName = jobName;
				sched.ScheduleJob(trigger);
			}

		}



		public static void MessageLogging(string logMessage)
		{
			try
			{
				string path = "~/MessageLog/" + DateTime.Today.ToString("dd-mm-yy") + ".txt";
				if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
				{
					File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
				}
				using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
				{
					string log = Membership.GetUser().UserName + " on " + String.Format("{0:d MMM yyyy HH:mm }", DateTime.Now);
					w.WriteLine(log);
					log = "Log Message: " + logMessage;
					w.WriteLine(log);
					w.WriteLine("__________________________");
					w.Flush();
					w.Close();
				}
			}
			catch (Exception ex)
			{
				MessageLogging(ex.Message);
			}

		}


	
	}
}
