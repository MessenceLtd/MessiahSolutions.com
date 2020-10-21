using AngleSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Umbraco.Web.HealthCheck.Checks.Permissions;

namespace MessiahSolutionsComFrontEnd
{
    public class WebsiteOperationsTools
    {
        public enum LanguageCode { en , he };

        public static void SendContactUsEmailDetails( 
            string fromUrl , string fullName , string emailAddress, string message )
        {
            LanguageCode languageCode = LanguageCode.en;
            if (fromUrl.IndexOf("/he/") > -1)
                languageCode = LanguageCode.he;

            string contactUsTemplateFileName = (languageCode == LanguageCode.en ? "ContactUs-en.html" : "ContactUs-he.html");
            string contactUsTemplateFilePath = GetWebsiteServerRootPhysicalPath() + @"EmailTemplates\" + contactUsTemplateFileName;

            string templateFileContent = File.ReadAllText(contactUsTemplateFilePath);

            // Get the appropriate html template from email templates folder and send the email to the configured address.
            Dictionary<string, string> emailTemplateKeyValues = new Dictionary<string, string>();
            emailTemplateKeyValues.Add("fullname", fullName);
            emailTemplateKeyValues.Add("email", emailAddress);
            emailTemplateKeyValues.Add("description", message);
            emailTemplateKeyValues.Add("fromUrl", fromUrl);

            string toMailMessage = ConfigurationManager.AppSettings["ContactUsEmailToRecipients"];
            string fromMailMessage = ConfigurationManager.AppSettings["SMTP_DefaultFromMessage"];

            EmailHelperSender(templateFileContent, emailTemplateKeyValues, toMailMessage, fromMailMessage, "New Contact-US Form From website!");
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

            mailMessage.IsBodyHtml = true;

            client.Send(mailMessage);
        }

        private static string GetWebsiteServerRootPhysicalPath()
        {
            return HttpContext.Current.Server.MapPath("~/");
        }
    }
}