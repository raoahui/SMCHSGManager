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
	public class PenangAshramJob : IJob
	{
		public static DateTime JobStartTime = new DateTime(2012, 1, 28);
		public static DateTime nextTrigger;

		private SMCHDBEntities _entities = new SMCHDBEntities();

		#region IJob Members

		public void Execute(IJobExecutionContext context)
		{
			try
			{
				//Do things here
				Debug.WriteLine("Scheduler is a success :" + DateTime.Now.ToString());
				nextTrigger = context.Trigger.GetNextFireTimeUtc().GetValueOrDefault().DateTime.AddHours(8);
				Debug.WriteLine("Next trigger fire time is " + context.Trigger.GetNextFireTimeUtc().ToString());

				//SaveContentAndSendEmail(3);
				
			}
			catch (JobExecutionException )
			{
				Debug.WriteLine("Scheduler Job Exception");
			}

		}

		#endregion

		public static string[,] GenerateExcelData(int ashramID)
		{
			string[,] contents = new string [1, 1];

			SMCHDBEntities _entities = new SMCHDBEntities();

			//var viewModel = _entities.InternationalGMApplicationInfos.Where(a=>a.AshramID == ashramID && a.ApplicationStatusID == 1).OrderByDescending(a => a.ID).ToList();
			var viewModel = GetLatestInternationalApplicationStatus(2, false);  // Screening approved.
			viewModel = viewModel.Where(a => a.InternationalGMApplicationInfo.AshramID == ashramID).ToList();

			if (viewModel.Count > 0)
			{
				if (ashramID == 3) // penang
				{
					contents = new string[viewModel.Count, 36+1];
					int i=0;
					foreach (InternationalGMApplicationInfo1 aigm1 in viewModel)
					{
						int j = 0;
						InternationalGMApplicationInfo aigm = aigm1.InternationalGMApplicationInfo;
                        AshramAndCenterInfo ashramAndCenterInfo = aigm.MemberInfo.AshramAndCenterInfo;
                        //MembershipUser user = Membership.GetUser(ashramAndCenterInfo.Name);
                        MembershipUser user = Membership.GetUser(aigm.MemberID);
                        contents[i, j++] = aigm.ID.ToString();
						contents[i, j++] = (i + 1).ToString();
						//contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", aigm.ApplyDate);
                        contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", DateTime.Today.ToUniversalTime().AddHours(8));
                        string CPName = Roles.GetUsersInRole("Contact Person").FirstOrDefault();
                        string CPEmail = Membership.GetUser(CPName).Email;
                        contents[i, j++] = ashramAndCenterInfo.Email + "; " + CPEmail;
						contents[i, j++] = null;
						contents[i, j++] = ashramAndCenterInfo.Country.Name;
						contents[i, j++] = ashramAndCenterInfo.Name;
						contents[i, j++] = aigm.MemberInfo.CountryOfBirth; 
						contents[i, j++] = aigm.MemberInfo.AshramAndCenterInfo.Name;
						contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", aigm.ArrivalDate);
						contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", aigm.DepartureDate);
						contents[i, j++] = aigm.MemberInfo.IDCardNo;
						contents[i, j++] = aigm.MemberInfo.Name;
						contents[i, j++] = aigm.MemberInfo.Gender.Name.Substring(0, 1);
						if (aigm.MemberInfo.DateOfBirth.HasValue)
						{
							contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", aigm.MemberInfo.DateOfBirth.Value);
							contents[i, j++] = GetAge(aigm.MemberInfo.DateOfBirth.Value).ToString();
						}
						else{
							j += 2;
						}
						if (aigm.MemberInfo.DateOfInitiation.HasValue)
						{
							contents[i, j] = string.Format("{0: dd-MMM-yyyy}", aigm.MemberInfo.DateOfInitiation.Value);
						}
						j++;
						contents[i, j++] = aigm.MemberInfo.InitiateType.Name.Substring(0, 1);

						List<InternationalTransport> transportStations = _entities.InternationalTransports.Where(a => a.AshramID == ashramID).ToList();
						foreach (InternationalTransport ts in transportStations)
						{

							if (_entities.InternationalGMApplicationTransportInfos.Any(a => a.InternationalGMApplicationInfoID == aigm.ID && a.InternationalTransportID == ts.ID && a.InBound))
							{
								InternationalGMApplicationTransportInfo transportInfo = _entities.InternationalGMApplicationTransportInfos.Single(a => a.InternationalGMApplicationInfoID == aigm.ID && a.InternationalTransportID == ts.ID && a.InBound);
								contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", transportInfo.DateTime);
								if (ts.StationName.ToLower().Contains("airport"))
								{
									contents[i, j++] = transportInfo.FlightNo;
								}
								contents[i, j++] = string.Format("{0: HH:mm}", transportInfo.DateTime);
							}
							else
							{
								j = j + 2;
								if (ts.StationName.ToLower().Contains("airport"))
									j++;
							}

							if (_entities.InternationalGMApplicationTransportInfos.Any(a => a.InternationalGMApplicationInfoID == aigm.ID && a.InternationalTransportID == ts.ID && !a.InBound))
							{
								InternationalGMApplicationTransportInfo transportInfo = _entities.InternationalGMApplicationTransportInfos.Single(a => a.InternationalGMApplicationInfoID == aigm.ID && a.InternationalTransportID == ts.ID && !a.InBound); 
								contents[i, j++] = string.Format("{0: dd-MMM-yyyy}", transportInfo.DateTime);
								if (ts.StationName.ToLower().Contains("airport"))
								{
									contents[i, j++] = transportInfo.FlightNo;
								}
								contents[i, j++] = string.Format("{0: HH:mm}", transportInfo.DateTime);
							}
							else
							{
								j = j + 2;
								if (ts.StationName.ToLower().Contains("airport"))
									j++;
							}
						}
						contents[i++, j] = aigm.Remark;
					}
				}
				else if (ashramID == 2 || ashramID == 4)
				{

				}
	
				
			}
			return contents;
		}

		public static List<InternationalGMApplicationInfo1> GetLatestInternationalApplicationStatus(int applicationStatusID, bool getAll)
		{
			SMCHDBEntities _entities = new SMCHDBEntities();
			int allID = _entities.ApplicationStatuses.Count() + 1;
			DateTime theDayAfterTommorrow = DateTime.Today.AddDays(2).ToUniversalTime().AddHours(8).AddHours(8);
			var latestInternationalGMApplicationStatus = from r in _entities.InternationalGMApplicationStatus
														 group r by r.InternationalGMApplicationInfoID into temp
														 let latestStatusID = temp.Max(a => a.ApplicationStatusID)
														 from r in temp
														 where r.ApplicationStatusID == latestStatusID && (r.InternationalGMApplicationInfo.ArrivalDate > theDayAfterTommorrow && !getAll || getAll)
														 select r;
			var viewModel = (from r in _entities.InternationalGMApplicationInfos
							 join h in latestInternationalGMApplicationStatus on r.ID equals h.InternationalGMApplicationInfoID
							 where (h.ApplicationStatusID == applicationStatusID || applicationStatusID == allID)
							 orderby r.ID
							 select new InternationalGMApplicationInfo1
							 {
								 InternationalGMApplicationInfo = r,
								 //ID = r.ID,
								 //MemberID = r.MemberID,
								 //AshramID = r.AshramID,
								 //ArrivalDate = r.ArrivalDate,
								 //DepartureDate = r.DepartureDate,
								 //ApplyDate = r.ApplyDate,
								 //Remark = r.Remark,
								 //ParentID = r.ParentID,
								 ApplicationStatus = h.ApplicationStatus.Name,
							 }).ToList();

			return viewModel;
		}

		public static int GetAge(DateTime birthDate)
		{
			DateTime n = DateTime.Now; // To avoid a race condition around midnight
			int age = DateTime.Now.Year - birthDate.Year;

			if (n.Month < birthDate.Month || (n.Month == birthDate.Month && n.Day < birthDate.Day))
				age--;

			return age;
		}

		public static void SaveContentAndSendEmail(int ashramID)
		{

			string[,] content = GenerateExcelData(ashramID);
			if (content.GetLength(1) <= 1)
			{
				return;
			}

			StringBuilder sbHeader = GetHeaderStringBuilder(ashramID);
			StringBuilder sb = GetDataStringBuilder(content);

            string excelFilePath = @"C:\SMCH\Penang Ashram GM Application Form (Spore) " + string.Format("{0: dd-MMM-yyyy}", DateTime.Today.ToUniversalTime().AddHours(8)) + ".xls";

			EmailMessage em = new EmailMessage();
			SMCHDBEntities _entities = new SMCHDBEntities();

			AshramAndCenterInfo ashramAndCenterInfo = _entities.AshramAndCenterInfos.SingleOrDefault(a => a.ID == ashramID);
			//"Attending Group Meditation At " + Penang Ashram [Feb 2012] (Singapore Centre, 2012-Jan-31)"
            em.Subject = "Attending Group Meditation At " + ashramAndCenterInfo.Name + " (Singapore Centre, " + string.Format("{0: dd-MMM-yyyy}", DateTime.Today.ToUniversalTime().AddHours(8)) + ")";

//            Hallo Noble Saint Yeap, 
//CP of Penang Centre,
 
//Kindly refer to the attached application form for the month of February 2012 revision 3.
//A new applicant is reflected in row 4.

//Brother Quek Chin Chye does attend group meditation at Singapore Centre.
//With your kind permission, please allow him to attend group meditation over there while he is in Penang.

//Kindly acknowledge receipt of this message.
//Thank you.


//Wishing you Master's Love & Blessings,
//Singapore Centre
            string temp = ashramAndCenterInfo.Name.Replace("Ashram", "");
            em.Message = "Hallo Noble Saint " + ashramAndCenterInfo.CPname + "\r\nContact Person of " + temp + "Center,\r\n";
            //em.Message += "\r\nKindly refer to the attached application form from " + string.Format("{0: dd-MMM-yyyy}", DateTime.Today.AddDays(-3)) + " To " + string.Format("{0: dd-MMM-yyyy}", DateTime.Today.AddDays(-1)) + "\r\n";
            em.Message += "\r\nKindly refer to the attached application form dated " + string.Format("{0: dd-MMM-yyyy}", DateTime.Today.ToUniversalTime().AddHours(8)) + ".\r\n";

            em.Message += "\r\nTotal applicants = " + content.GetLength(0).ToString() + ".";
            em.Message += "\r\nThey do attend group meditation at Singapore Centre." + "\r\nWith your kind permission, please allow them to attend group meditation over there while they are in Penang.\r\n";
			em.Message += "\r\nKindly acknowledge receipt of this message.\r\nThank you.\r\n";
			em.Message += "\r\nWishing you Master's Love & Blessings,\r\nSingapore Centre\r\n";

            em.From = "admin@smchsg.com";

            string CPName = Roles.GetUsersInRole("Contact Person").FirstOrDefault();
            string CPEmail = Membership.GetUser(CPName).Email;

            string localCenterEmail = _entities.AshramAndCenterInfos.SingleOrDefault(a => a.ID == 1).Email; // "Singapore Center"

            em.To = localCenterEmail;   //ashramAndCenterInfo.Email
            em.cc = localCenterEmail + ", " + CPEmail;
            em.bcc = "admin@smchsg.com";

			em.AttachFilePath = excelFilePath;
			//em.sb = sb;
			em.AttachedString = sbHeader.ToString() + sb.ToString();

			EmailService es = new EmailService();
			try
			{
				es.SendMessage(em);
				// change status
				for (int i = 0; i < content.GetLength(0); i++)
				{
					AddApplicationStatus(_entities, int.Parse(content[i, 0]), 4);
				}
			}
			catch
			{
			}

		}

		public static void AddApplicationStatus(SMCHDBEntities _entities, int ID, int statusID)
		{
			InternationalGMApplicationStatu InternationalGMApplicationStatus = new InternationalGMApplicationStatu();
			InternationalGMApplicationStatus.ApplicationStatusID = statusID;
			InternationalGMApplicationStatus.ConfirmDate = DateTime.Now.ToUniversalTime().AddHours(8);
			InternationalGMApplicationStatus.InternationalGMApplicationInfoID = ID;
			_entities.AddToInternationalGMApplicationStatus(InternationalGMApplicationStatus);
			_entities.SaveChanges();
		}

		private static StringBuilder GetHeaderStringBuilder(int ashramID)
		{
			StringBuilder sbHeader = new StringBuilder();

			sbHeader.Append("<table cellpadding='2' border='2' background-color='Gray' > ");

			sbHeader.Append("<tr>");
			sbHeader.Append("<th>No.</th>");
			sbHeader.Append("<th>Application<br/> Date</th>");
			sbHeader.Append("<th>CP/Center E-mail Address</th>");
			sbHeader.Append("<th>Penang Reply<br/> Date</th>");
			sbHeader.Append("<th>CP/Center<br/> Country</th>");
			sbHeader.Append("<th>Contact Person<br/> Center</th>");
			sbHeader.Append("<th>Applicant's <br/>Country of Birth</th>");
			sbHeader.Append("<th>Applicant's <br/>Attending GM Center</th>");
			sbHeader.Append("<th colspan='2'>Duration of Stay<br/>逗留期限(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th colspan='2'>ID Card<br/>識別證</th>");
			sbHeader.Append("<th rowspan='3' style='text-align:center'>Gender <br/>(M/F)<br/>性別<br/>(男/女)</th>");
			sbHeader.Append("<th rowspan='3' style='text-align:center'>Date of Birth<br/>出生日<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th rowspan='3' style='text-align:center'>Age<br/>年齡</th>");
			sbHeader.Append("<th rowspan='3' style='text-align:center'>Date of <br/>Initiation<br/>印心日<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th rowspan='3' style='text-align:center'>Full (F)<br/>or <br/>Half (H)<br/> Initiate</th>");
			sbHeader.Append("<th colspan='6'>Airport<br/>飞机场</th>");
			sbHeader.Append("<th colspan='4'>Sungai Nibong Bus Station<br/>巴士车站</th>");
			sbHeader.Append("<th colspan='4'>Ferry Terminal<br/>渡轮码头</th>");
			sbHeader.Append("<th colspan='4'>Balik Pulau Bus Station<br/>巴士车站</th>");
			sbHeader.Append("<th rowspan='3'>Notes 備註 </th>");
			sbHeader.Append("</tr>");


			sbHeader.Append("<tr>");
			sbHeader.Append("<th rowspan='2'>序號</th>");
            sbHeader.Append("<th rowspan='2'>小中心<br/>推薦日</th>");
			sbHeader.Append("<th rowspan='2'>联络人/小中心信箱</th>");
			sbHeader.Append("<th rowspan='2'>穦城回函日</th>");
			sbHeader.Append("<th rowspan='2'>联络人國家</th>");
			sbHeader.Append("<th rowspan='2'>联络人小中心</th>");
			sbHeader.Append("<th rowspan='2'>申請者<br/>出生國家</th> ");
			sbHeader.Append("<th rowspan='2'>申請者<br/>共修小中心</th> ");
			sbHeader.Append("<th rowspan='2'>Start Date<br/>开始日期</th>");
			sbHeader.Append("<th rowspan='2'>End Date<br/>结束日期</th>");
			sbHeader.Append("<th rowspan='2'>8-digit Number<br/>八位編號</th>");
			sbHeader.Append("<th rowspan='2'>Full Name<br/>姓名</th>");
			sbHeader.Append("<th colspan='3'>Arrival 到达</th>");
			sbHeader.Append("<th colspan='3'>Departure 出发</th>");
			sbHeader.Append("<th colspan='2'>Arrival 到达</th>");
			sbHeader.Append("<th colspan='2'>Departure 出发</th>");
			sbHeader.Append("<th colspan='2'>Arrival 到达</th>");
			sbHeader.Append("<th colspan='2'>Departure 出发</th>");
			sbHeader.Append("<th colspan='2'>Arrival 到达</th>");
			sbHeader.Append("<th colspan='2'>Departure 出发</th>");
			sbHeader.Append("</tr>");

			sbHeader.Append("<tr>");
			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>DFlight No.<br/>航班号</th>");
			sbHeader.Append("<th>Arrival <br/>Time 到达时间</th>");
			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>DFlight No.<br/>航班号</th>");
			sbHeader.Append("<th>Departure<br/>Time 出发时间</th>");

			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>Arrival <br/>Time >到达时间</th>");
			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>Departure<br/>Time 出发时间</th>");

			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>Arrival <br/>Time 到达时间</th>");
			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>Departure<br/>Time 出发时间</th>");

			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>Arrival <br/>Time 到达时间</th>");
			sbHeader.Append("<th>Date 日期<br/>(dd-mmm-yyyy)</th>");
			sbHeader.Append("<th>Departure<br/>Time 出发时间</th>");
			sbHeader.Append("</tr>");

			sbHeader.Append("</table>");

			return sbHeader;
		}

		private static StringBuilder GetDataStringBuilder(string[,] content)
		{
			Table table = new Table();

			StringBuilder sb = new StringBuilder();
			using (StringWriter sw = new StringWriter(sb))
			{
				using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
				{
					for (int i = 0; i < content.GetLength(0); i++)
					{
						TableRow r = new TableRow();
						for (int j = 1; j < content.GetLength(1); j++)
						{
							TableCell c = new TableCell();
							c.Controls.Add(new System.Web.UI.LiteralControl(content[i, j]));
							r.Cells.Add(c);
							
						}
						table.Rows.Add(r);
					}

					table.GridLines = GridLines.Both;
				
					//  render table
					table.RenderControl(htw);
				}
			}
			return sb;
		}

		//private static string[][] GetHeaders(int ashramID)
		//{

		//    string[][] headers = new string[1][]
		//            {
		//                new string[] {
		//                    "No.", "Application Date", "CP/Center E-mail", "Penang Reply Date", 
		//                    "CP/Center Country", "Contact Person Center", "Applicant's Origin Country", "Applicant's Attending GM Center", 
		//                    "Start Date", "End Date", "ID Card", "Gender", "Date of Birth", "Date of Initiation", "Full/Half Initiate",
		//                    "Airport Arrival Date", "Airport Arrival Flight No.", "Airport Arrival Time", 
		//                    "Airport Departure Date", "Airport Departure Flight No.", "Airport Departure Time",
		//                    "Sungai Nibong Bus Station Arrival Date", "Sungai Nibong Bus Station Arrival Time", 
		//                    "Sungai Nibong Bus Station Departure Date", "Sungai Nibong Bus Station Departure Time",
		//                    "Ferry Terminal Arrival Date", "Ferry Terminal Arrival Time",
		//                    "Ferry Terminal Departure Date", "Ferry Terminal Departure Time",
		//                    "Balik Pulau Bus Station Arrival Date", "Balik Pulau Bus Station Arrival Time",
		//                    "Balik Pulau Bus Station Departure Date", "Balik Pulau Bus Station Departure Time", 
		//                    "Remark",
		//                },
		//            };
		//    return headers;
		//}


		//private static Workbook mWorkBook;
		//private static Sheets mWorkSheets;
		//private static Worksheet mWSheet1;
		//private static Application oXL;

		//public static void InsertContentToExistExcel(int ashramID)
		//{
		//    //string headFilePath = @"C:\SMCH\Penang Ashram GM Application Form.xls";
		//    string headFilePath = @"../Penang Ashram GM Application Form.xls";
		//    if (ashramID == 2 || ashramID == 4)
		//    {
		//        headFilePath = @"C:\SMCH\...";
		//    }
		//    oXL = new Microsoft.Office.Interop.Excel.Application();
		//    oXL.Visible = true;
		//    oXL.DisplayAlerts = false;

		//    Workbooks workbooks = oXL.Workbooks;
		//    mWorkBook = workbooks.Open(headFilePath, 0, false, 5, null, null, false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, null, true, false, 0, true, false, false);

		//    //Get all the sheets in the workbook
		//    mWorkSheets = mWorkBook.Worksheets;

		//    //Get the allready exists sheet
		//    mWSheet1 = (Microsoft.Office.Interop.Excel.Worksheet)mWorkSheets.get_Item("Sheet1");
		//    Microsoft.Office.Interop.Excel.Range range = mWSheet1.UsedRange;
		//    int colCount = range.Columns.Count;
		//    int headCount = range.Rows.Count;

		//    string[,] content = GenerateExcelData(ashramID, colCount);

		//    for (int index = 1; index <= content.GetLength(0); index++)
		//    {
		//        mWSheet1.Cells[headCount + index, 1] = index;
		//        for (int colNo = 1; colNo <= colCount; colNo++)
		//        {
		//            mWSheet1.Cells[headCount + index, colNo+1] = content[index-1, colNo-1];
		//        }
		//    }

		//    string excelFilePath = headFilePath.Substring(0, headFilePath.Length-4) +  string.Format("{0: dd-MMM-yyyy}", DateTime.Today) + ".xls";
		//    mWorkBook.SaveAs(excelFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
		//    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
		//    Missing.Value, Missing.Value, Missing.Value,
		//    Missing.Value, Missing.Value);

		//    mWorkBook.Close(Missing.Value, Missing.Value, Missing.Value);

		//    mWSheet1 = null;
		//    mWorkBook = null;

		//    oXL.Quit();

		//    GC.WaitForPendingFinalizers();
		//    GC.Collect();

		//    GC.WaitForPendingFinalizers();
		//    GC.Collect();


		//    EmailMessage em = new EmailMessage();
		//    SMCHDBEntities _entities = new SMCHDBEntities();

		//    AshramAndCenterInfo ashramAndCenterInfo = _entities.AshramAndCenterInfos.SingleOrDefault(a=>a.ID == ashramID);
		//    em.Subject = "Application Form for Initiates Attending Group Meditation at " + ashramAndCenterInfo.Name;

		//    //em.Message = message; // + ". " + em.Message;
		//    em.From =  "chinghai@singnet.com.sg"; 
		//    em.To = "admin@smchsg.com"; //ashramAndCenterInfo.Email
		//    em.AttachFilePath = excelFilePath;

		//    EmailService es = new EmailService();
		//    es.SendMessage(em);

		//}

	}

}

