using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TrackingJob.Model
{
    class Mailer
    {
        // send email with attachment
        public static string SendEmail ( string to, string subject, string emailBody )
        {
            try {
                //to = ConfigurationManager.AppSettings["to"].ToString(); //To address    
                //to = to; //To address    
                string from = ConfigurationManager.AppSettings["from"].ToString(); //From address
                string smtp = ConfigurationManager.AppSettings["smtp"].ToString(); //smtp
                int port = Int32.Parse(ConfigurationManager.AppSettings["port"].ToString()); //port
                MailMessage message = new MailMessage(from, to);

                //string mailbody = "<b>This is a test from OUATTARA host.</b>";
                string mailbody = emailBody;
                //mailbody = emailBody;
                message.Subject = subject;
                // html true
                message.IsBodyHtml = true;
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(smtp, port); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(from, ConfigurationManager.AppSettings["password"].ToString());
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                client.Send(message);
                return "email sent from " + from + " to " + to;
            }
            catch ( Exception ex ) {
                Console.WriteLine( "Unable to send email. Error : " + ex );
                return "Error while sending email";
            }
        }
        
        // send email with attachment
        public static string SendEmailWithCopy ( string to, string subject, string emailBody, string cc )
        {
            try {
                //to = ConfigurationManager.AppSettings["to"].ToString(); //To address    
                //to = to; //To address    
                string from = ConfigurationManager.AppSettings["from"].ToString(); //From address
                string smtp = ConfigurationManager.AppSettings["smtp"].ToString(); //smtp
                int port = Int32.Parse(ConfigurationManager.AppSettings["port"].ToString()); //port
                MailMessage message = new MailMessage(from, to);

                //string mailbody = "<b>This is a test from OUATTARA host.</b>";
                string mailbody = emailBody;
                //mailbody = emailBody;
                message.Subject = subject;
                // html true
                message.CC.Add(cc);
                message.IsBodyHtml = true;
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(smtp, port); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(from, ConfigurationManager.AppSettings["password"].ToString());
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                client.Send(message);
                return "email sent from " + from + " to " + to;
            }
            catch ( Exception ex ) {
                Console.WriteLine( "Unable to send email. Error : " + ex );
                return "Error while sending email";
            }
        }

        public static string SendEmail()
        {
            string to = ConfigurationManager.AppSettings["to"].ToString(); //To address    
            string from = ConfigurationManager.AppSettings["from"].ToString(); //From address
            string smtp = ConfigurationManager.AppSettings["smtp"].ToString(); //smtp
            int port = Int32.Parse(ConfigurationManager.AppSettings["port"].ToString()); //port
            MailMessage message = new MailMessage(from, to);

            string mailbody = "This is a test from OUATTARA host.";
            message.Subject = "test afriland email";
            // html true
            message.IsBodyHtml = true;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(smtp, port); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential(from, ConfigurationManager.AppSettings["password"].ToString());
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
                return "email sent from " + from + " to " + to;
            }

            catch (Exception ex)
            {
                throw ex;
                return "Erreur " + ex.Message;
            }
        }
    }
}
