using AngleSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MessiahSolutionsComFrontEnd
{
    public class WebsiteOperationsTools
    {
        public static void SendContactUsEmailDetails( string fromUrl , string fullName , string emailAddress, string message )
        {
            // Get the appropriate html template from email templates folder and send the email to the configured address.

            Dictionary<string, string> emailTemplateKeyValues = new Dictionary<string, string>();
            emailTemplateKeyValues.Add("fullname", fullName);
            emailTemplateKeyValues.Add("emailAddress", emailAddress);
            emailTemplateKeyValues.Add("description", message);
            emailTemplateKeyValues.Add("fromUrl", fromUrl);

            string toMailMessage = "vadim@messiahsolutions.com";
            string fromMailMessage = "vadim@messiahsolutions.com";
            //Environment.CurrentDirectory;

            EmailHelperSender("test", emailTemplateKeyValues, toMailMessage, fromMailMessage, "test subject");

            
            


        }

        private static void EmailHelperSender( string emailTemplateContentAsBodyMessage, 
            Dictionary<string, string> emailTemplateKeyValues , string emailAddressTo , 
            string emailAddressFrom, string emailSubject )
        {
            foreach (string key in emailTemplateKeyValues.Keys)
            {
                if (emailTemplateContentAsBodyMessage.IndexOf("{{" + key + "}}") > -1 )
                {
                    emailTemplateContentAsBodyMessage = emailTemplateContentAsBodyMessage.Replace("{{" + key + "}}", emailTemplateKeyValues[key]);
                }
            }

            SmtpClient client = new SmtpClient();

            client.Host = ConfigurationManager.AppSettings["SMTP_Server"];
            client.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["SMTP_UserName"], 
                ConfigurationManager.AppSettings["SMTP_Password"]);
            client.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SMTP_EnableSsl"]);

            MailMessage mailMessage = new MailMessage(emailAddressFrom, emailAddressTo, emailSubject, emailTemplateContentAsBodyMessage);

            client.Send(mailMessage);
        }
    }
}