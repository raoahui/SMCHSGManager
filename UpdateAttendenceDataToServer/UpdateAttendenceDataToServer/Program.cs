using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace UpdateAttendenceDataToServer
{
	class Program
	{
        static SMCHDBEntities _entities = new SMCHDBEntities();
        static void Main(string[] args)
		{
			System.Console.WriteLine("Please Wait....");
			//GetCheckInOutAccessTableData(args);
            //CorrectGroupMeditationSchedule()
            CreateNextYearGMEvent();
			System.Console.WriteLine("Done!");
			System.Console.ReadLine();
		}

        static private void GetCheckInOutAccessTableData(string[] args)
		{
			//SMCHDBEntities _entities = new SMCHDBEntities();
           
            string conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Program Files\\att2008\\att2000.mdb"; 
			string strSql = "SELECT CHECKINOUT.CHECKTIME, USERINFO.Badgenumber FROM (CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID) ";

			OleDbConnection con = new OleDbConnection(conString);

			DataTable AttendanceTable = new DataTable();

			System.Console.WriteLine("Opening MDB connection...");
			con.Open();
			
			OleDbDataAdapter dAdapter = new OleDbDataAdapter();
			dAdapter.SelectCommand = new OleDbCommand(strSql, con);
			dAdapter.Fill(AttendanceTable);
			con.Close();

			System.Console.WriteLine("Data loaded from MDB file. Updating database, please wait...");

            // get latest time in GroupMeditationAttendance table
            DateTime latestGroupMeditationAttendanceDB = _entities.GroupMeditationAttendances.OrderByDescending(a => a.CheckInTime).Select(a => a.CheckInTime).FirstOrDefault();
			if (latestGroupMeditationAttendanceDB < new DateTime(2011, 12, 1) || args.Length == 1 && args[0].ToLower() == "all")
			{
				latestGroupMeditationAttendanceDB = new DateTime(2011, 12, 1);
			}
			else
			{
				latestGroupMeditationAttendanceDB = latestGroupMeditationAttendanceDB.AddDays(-3);
			}
            
            // Filter old record and no member person
            //var attendanceList = AttendanceTable.AsEnumerable().Where(
            //    a => a.Field<DateTime>("CHECKTIME") >= latestGroupMeditationAttendanceDB).ToList();
            var attendanceList = AttendanceTable.AsEnumerable().Where(
                a => a.Field<DateTime>("CHECKTIME") >= latestGroupMeditationAttendanceDB).OrderBy(a=>a.Field<DateTime>("CHECKTIME")).ToList();

            List<int> memberNoList = _entities.MemberInfos.Where(a => a.MemberNo.HasValue).Select(a => a.MemberNo.Value).ToList();

			foreach (DataRow dr in attendanceList)
			{
				DateTime checkInTime = (DateTime)dr["CHECKTIME"];

				int memberNo = int.Parse((string)dr["Badgenumber"]);
				if (!memberNoList.Contains(memberNo))
				{
					continue;
				}

				//System.Console.WriteLine("Uploading record of member " + memberNo.ToString() + " at " + checkInTime.ToString());

				Guid memberID = _entities.MemberInfos.SingleOrDefault(a => a.MemberNo == memberNo).MemberID;

				DateTime checkInTimeStart = checkInTime.AddMinutes(90+15);
                DateTime checkInTimeEnd = checkInTime.AddMinutes(-15);
				//if (_entities.Events.Any(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime))
                if (_entities.Events.Any(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTimeEnd))
				{
					//int eventID = _entities.Events.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime).FirstOrDefault().ID;
                    Event aEvent = _entities.Events.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTimeEnd).FirstOrDefault();

                    if (!_entities.EventRegistrations.Any(a => a.EventID == aEvent.ID && a.MemberID == memberID))
					{
						EventRegistration eventRegistration = new EventRegistration();
						eventRegistration.MemberID = memberID;
						eventRegistration.EventID = aEvent.ID;
						eventRegistration.SignTime = checkInTime;
						_entities.AddToEventRegistrations(eventRegistration);
						_entities.SaveChanges();

                        System.Console.WriteLine("Uploading" + aEvent.Title + " MemberNo: " + memberNo.ToString() + " at " + checkInTime.ToString());
                        //System.Console.WriteLine("Updated Event checkin");
						continue;
					}
				}

                //if (_entities.GroupMeditations.Any(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime))
                if (_entities.GroupMeditations.Any(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTimeEnd))
                {
					//int groupMeditaionID = _entities.GroupMeditations.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTime).FirstOrDefault().ID;
                    int groupMeditaionID = _entities.GroupMeditations.Where(a => a.StartDateTime <= checkInTimeStart && a.StartDateTime >= checkInTimeEnd).FirstOrDefault().ID;

					if (!_entities.GroupMeditationAttendances.Any(a => a.GroupMeditationID == groupMeditaionID && a.MemberID == memberID))
					{
						GroupMeditationAttendance groupMeditationAttendance = new GroupMeditationAttendance();
						groupMeditationAttendance.MemberID = memberID;
						groupMeditationAttendance.GroupMeditationID = groupMeditaionID;
						groupMeditationAttendance.CheckInTime = checkInTime;
						_entities.AddToGroupMeditationAttendances(groupMeditationAttendance);
						_entities.SaveChanges();

                        System.Console.WriteLine("Uploading GM: MemberNo " + memberNo.ToString() + " at " + checkInTime.ToString());
  					}
				}
			}

		}

        static private void CorrectGroupMeditationSchedule()
        {
            // Correct minutes from 1/1/2017 for night GM   Apr 8 2017
            DateTime newDay = new DateTime(2017, 1, 1);
            List<GroupMeditation> GMs = _entities.GroupMeditations.Where(a => a.StartDateTime >= newDay && a.StartDateTime.Hour == 23 && a.StartDateTime.Minute == 00 && a.StartDateTime.Second == 0).ToList();
            foreach (GroupMeditation gm in GMs)
            {
                gm.StartDateTime = gm.StartDateTime.AddMinutes(59);
                _entities.SaveChanges();
            }
        }

        static private void CreateNextYearGMEvent()
        {
            int year = DateTime.Today.Year + 1;
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        AddGroupMeditationItem(dt, 9, 3, 1);
                        break;
                    case DayOfWeek.Monday:
                        AddGroupMeditationItem(dt, 19.5, 2, 1);
                        break;
                    case DayOfWeek.Tuesday:
                        AddGroupMeditationItem(dt, 19.5, 2, 1);
                        break;
                    case DayOfWeek.Wednesday:
                        AddGroupMeditationItem(dt, 24, 6, 1);
                        break;
                    case DayOfWeek.Thursday:
                        AddGroupMeditationItem(dt, 19.5, 2, 1);
                        break;
                    case DayOfWeek.Friday:
                        AddGroupMeditationItem(dt, 20, 0.5, 3);
                        break;
                    case DayOfWeek.Saturday:
                        AddGroupMeditationItem(dt, 9, 3, 1);
                        AddGroupMeditationItem(dt, 19, 2.5, 1);
                        AddGroupMeditationItem(dt, 24, 7, 1);
                        break;
                    default:
                        break;
                }
            }
        }

        static private void AddGroupMeditationItem(DateTime dt, double startTime, double duration, int InitiateTypeID)
        {
            GroupMeditation gm = new GroupMeditation();
            gm.InitiateTypeID = InitiateTypeID;
            gm.StartDateTime = dt.Date.AddHours(startTime);
            gm.EndDateTime = gm.StartDateTime.AddHours(duration);
            if (startTime == 24)
            {
                gm.StartDateTime = gm.StartDateTime.AddMinutes(-1);
            }

            if (!_entities.GroupMeditations.Any(a => a.StartDateTime == gm.StartDateTime))
            {
                _entities.AddToGroupMeditations(gm);
                _entities.SaveChanges();
            }

        }

        static public void CorrectGMEvent()
        {

            //var latestMemberFeePayments = (from r in _entities.MemberFeePayments
            //                               orderby r.ToDate descending
            //                               group r by r.MemberInfo.MemberID into h
            //                               select new
            //                               {
            //                                   MemberID = h.Key,
            //                                   ToDate = h.Max(a => a.ToDate),
            //                               }).ToList();

            //foreach (var mfp in latestMemberFeePayments)
            //{
            //    MemberInfo memberInfo = _entities.MemberInfos.Single(a => a.MemberID == mfp.MemberID);
            //    if (memberInfo.MemberFeeExpiredDate < mfp.ToDate)
            //    {
            //        memberInfo.MemberFeeExpiredDate = mfp.ToDate;
            //        _entities.SaveChanges();
            //    }
            // }

            //List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where (r.InitiateTypeID == 3 || r.StartDateTime.Hour == 9 && r.StartDateTime > new DateTime(2012, 2, 6)) orderby r.StartDateTime select r).ToList();

            //foreach (GroupMeditation gm in GMs)
            //{
            //    if (gm.StartDateTime.DayOfWeek == DayOfWeek.Sunday)
            //    {
            //        gm.EndDateTime = gm.EndDateTime.AddHours(1);
            //    }
            //    else if (gm.StartDateTime.DayOfWeek == DayOfWeek.Friday)
            //    {
            //        gm.StartDateTime = gm.StartDateTime.AddHours(0.5);
            //        gm.EndDateTime = gm.EndDateTime.AddHours(0.5);
            //    }
            //    _entities.SaveChanges();
            //}
            List<GroupMeditation> GMs = (from r in _entities.GroupMeditations
                                         where
                                             r.StartDateTime.Hour == 9
                                         orderby r.StartDateTime
                                         select r).ToList();

            foreach (GroupMeditation gm in GMs)
            {
                //if ( gm.StartDateTime.DayOfWeek == DayOfWeek.Sunday )
                //{
                //    if( gm.StartDateTime > new DateTime(2012, 2, 6)) 
                //    {
                //        gm.EndDateTime = gm.StartDateTime.AddHours(4);
                //    }
                //    else
                //    {
                //        gm.EndDateTime = gm.StartDateTime.AddHours(3);
                //    }
                //}
                if (gm.StartDateTime.DayOfWeek == DayOfWeek.Saturday && gm.StartDateTime > new DateTime(2015, 3, 1))
                {
                    gm.EndDateTime = gm.StartDateTime.AddHours(3);
                }
                _entities.SaveChanges();
            }
            //List<GroupMeditation> GMs = (from r in _entities.GroupMeditations where (r.StartDateTime.Hour == 0) orderby r.StartDateTime select r).ToList();

            //foreach (GroupMeditation gm in GMs)
            //{
            //    gm.StartDateTime = gm.StartDateTime.AddHours(-1);
            //    _entities.SaveChanges();
            //}

            //DateTime beginDate = new DateTime(2013, 1, 1); // DateTime.Today;
            //DateTime endDate = new DateTime(2013, 12, 31);

            //for (DateTime dt = beginDate; dt <= endDate; dt = dt.AddDays(1))
            //{
            //    GroupMeditation gm = new GroupMeditation();
            //    gm.InitiateTypeID = 1;
            //    gm.StartDateTime = dt.Date;
            //    gm.EndDateTime = gm.StartDateTime.AddHours(7);
            //    if (dt.DayOfWeek == DayOfWeek.Thursday)
            //    {
            //        gm.EndDateTime = gm.StartDateTime.AddHours(6);
            //        if (!_entities.GroupMeditations.Any(a => a.StartDateTime == gm.StartDateTime))
            //        {
            //            _entities.AddToGroupMeditations(gm);
            //            _entities.SaveChanges();
            //        }
            //    }
            //}

            ////for (DateTime dt = beginDate; DateTime.Compare(dt, endDate) <= 0; dt = dt.AddDays(1))
            ////{
            ////    if(dt.DayOfWeek != DayOfWeek.Wednesday && dt.DayOfWeek != DayOfWeek.Friday && !_entities.GroupMeditations.Any(a=>a.StartDateTime == dt))
            ////    {
            ////        GroupMeditation gm = new GroupMeditation();
            ////        gm.InitiateTypeID = 1;
            ////        gm.StartDateTime = dt.Date;
            ////        gm.EndDateTime = gm.StartDateTime.AddHours(7);

            ////        if (dt.DayOfWeek == DayOfWeek.Sunday) 
            ////        {
            ////            if (dt.DayOfWeek == DayOfWeek.Thursday)
            ////            {
            ////                gm.EndDateTime = gm.StartDateTime.AddHours(6);
            ////            }
            ////            _entities.AddToGroupMeditations(gm);
            ////            gm = new GroupMeditation();
            ////            gm.InitiateTypeID = 1;
            ////        }

            ////        if (dt.DayOfWeek == DayOfWeek.Monday || dt.DayOfWeek == DayOfWeek.Tuesday || dt.DayOfWeek == DayOfWeek.Thursday)
            ////        {
            ////            gm.StartDateTime = dt.Date.AddHours(19.5);
            ////            gm.EndDateTime = gm.StartDateTime.AddHours(2);
            ////        }
            ////        else if (dt.DayOfWeek == DayOfWeek.Saturday)
            ////        {
            ////            gm.StartDateTime = dt.Date.AddHours(19);
            ////            gm.EndDateTime = gm.StartDateTime.AddHours(2.5);
            ////        }
            ////        else if (dt.DayOfWeek == DayOfWeek.Sunday)
            ////        {
            ////            gm.StartDateTime = dt.Date.AddHours(9);
            ////            gm.EndDateTime = gm.StartDateTime.AddHours(3);
            ////        }
            ////        else
            ////        {
            ////            continue;
            ////        }

            ////        _entities.AddToGroupMeditations(gm);
            ////        _entities.SaveChanges();
            ////    }

            ////}			

        }

    
    }
}
