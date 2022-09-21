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
        public static string SendEmailWIthAttachment ( string to, string subject, string message )
        {
            try {
                string from_password = string.Empty;
                string from = string.Empty;
                int port = Int32.Parse(ConfigurationManager.AppSettings["port"].ToString());
                // get the parameters
                from = ConfigurationManager.AppSettings["emailFrom"].ToString();
                from_password = ConfigurationManager.AppSettings["from_password"].ToString();

                SmtpClient mailServer = new SmtpClient( "smtp.gmail.com", port );
                mailServer.UseDefaultCredentials = false;
                mailServer.EnableSsl = true;

                mailServer.Credentials = new System.Net.NetworkCredential( from, from_password );



                MailMessage msg = new MailMessage( from, to );
                msg.Subject = subject;
                msg.Body = message;
                //msg.Attachments.Add( new Attachment( "D:\\myfile.txt" ) );
                mailServer.Send( msg );
                return "Email sent from " + from + " to " + to;
            }
            catch ( Exception ex ) {
                Console.WriteLine( "Unable to send email. Error : " + ex );
                return "Error while sending email";
            }
        }
    }
}
