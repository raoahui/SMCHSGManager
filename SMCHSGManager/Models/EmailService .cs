using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Xml.Serialization;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.IO;
using System.Text;

namespace SMCHSGManager.Models
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
		public string AttachFilePath { get; set; }
		public string AttachedString { get; set; }
		public string cc { get; set; }
		public string bcc { get; set; }
    }

    public class EmailService
    {

        //public void sendEmail(EmailMessage em)
        //{
        //    //EmailMessage em = new EmailMessage();
        //    //em.Subject = "test message";
        //    //em.Message = "howdy from asp.net mvc";
        //    //em.From = "raohui1@gmail.com";
        //    //em.To = "raohui1@gmail.com";

        //    new EmailService().SendMessage(em,
        //        "Raohui1@gmail.com", //"{userName}",
        //        "chinghai", //"{password}",
        //        "smtp.gmail.com", //"{host}",
        //        587, //port,
        //        true);
        //}

        public void SendMessage(EmailMessage message)
        {
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~\\Web.config");
			
            //Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
       
            //Response.Write("from: " + settings.Smtp.From + "<br />");
            string username = settings.Smtp.Network.UserName;
            string password = settings.Smtp.Network.Password ;
            string host = settings.Smtp.Network.Host;
            int port = settings.Smtp.Network.Port;
            //bool enableSsl = true;
            bool enableSsl = false;

            MailMessage mm = new MailMessage(message.From, message.To, message.Subject, message.Message);
            NetworkCredential credentials = new NetworkCredential(username, password);
            SmtpClient sc = new SmtpClient(host,port);
            sc.EnableSsl = enableSsl;
			
            sc.Credentials = credentials;

			//Create Attachment
			if (!string.IsNullOrEmpty(message.AttachFilePath) && message.AttachedString != null)
			{

				byte[] myByteArray = System.Text.Encoding.UTF8.GetBytes(message.AttachedString.ToString());
				MemoryStream ms = new MemoryStream(myByteArray);
				mm.Attachments.Add(new Attachment(ms, message.AttachFilePath, "application/vnd.xls"));
				//mm.Attachments.Add(new Attachment(@"C:\SMCH\Penang Ashram GM Application Form.xls ", "application/vnd.xls"));
			}
			if (!string.IsNullOrEmpty(message.cc))
			{
				string[] cclist = message.cc.Split(',');
				for (int i = 0; i < cclist.Count(); i++)
				{
					mm.CC.Add(cclist[i]);
				}
			}
			if (!string.IsNullOrEmpty(message.bcc))
			{
				string[] cclist = message.bcc.Split(',');
				for (int i = 0; i < cclist.Count(); i++)
				{
					mm.Bcc.Add(cclist[i]);
				}
			}

            try
            {
                sc.Send(mm);
                //repository.LogMessage(serializedMessage, true);
            }
            catch (Exception)
            {
                throw;
                 //repository.LogMessage(serializedMessage, false);
            }

    
        }


    }
}

