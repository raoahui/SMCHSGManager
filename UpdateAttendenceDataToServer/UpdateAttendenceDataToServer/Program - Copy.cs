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
		static void Main(string[] args)
		{
			System.Console.WriteLine("Please Wait....");
			GetCheckInOutAccessTableData(args);
			System.Console.WriteLine("Done!");
			System.Console.ReadLine();
		}

        static private void GetCheckInOutAccessTableData(string[] args)
		{
			SMCHDBEntities _entities = new SMCHDBEntities();
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
            var attendanceList = AttendanceTable.AsEnumerable().Where(
                a => a.Field<DateTime>("CHECKTIME") >= latestGroupMeditationAttendanceDB);

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

	}
}
