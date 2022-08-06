using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net.Configuration;

namespace Messence.Shared.EmailTemple
{
    public class EmailSender
    {
        public readonly string c_Key_SmtpSettings_Server = "SMTP_Settings_Server";
        public readonly string c_Key_SmtpSettings_Port = "SMTP_Settings_Port";
        public readonly string c_Key_SmtpSettings_UseSSL = "SMTP_Settings_UseSSL";
        public readonly string c_Key_SmtpSettings_Username = "SMTP_Settings_Username";
        public readonly string c_Key_SmtpSettings_Password = "SMTP_Settings_Password";
        public readonly string c_Key_SmtpSettings_DeliveryMethod = "SMTP_Settings_DeliveryMethod";
        public readonly string c_Key_SmtpSettings_DeliveryFormat = "SMTP_Settings_DeliveryFormat";
        public readonly string c_Key_SmtpSettings_DefaultCredentials = "SMTP_Settings_DefaultCredentials";
        public readonly string c_Key_SmtpSettings_DefaultFromAddress = "SMTP_Settings_DefaultFromAddress";

        public string SmtpSettings_Server { get; set; }
        public int SmtpSettings_Port { get; set; }
        public bool SmtpSettings_UseSSL { get; set; }
        public string SmtpSettings_Username { get; set; }
        public string SmtpSettings_Password { get; set; }
        public bool SmtpSettings_DefaultCredentials { get; set; }
        public SmtpDeliveryMethod SmtpSettings_DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
        public SmtpDeliveryFormat SmtpSettings_DeliveryFormat { get; set; } = SmtpDeliveryFormat.International;
        public string SmtpSettings_DefaultFromAddress { get; set; }

        /// <summary>
        /// Default constructor that will initialize default values from configuration settings. 
        /// </summary>
        public EmailSender()
        {
            LoadSmtpSettingsFromConfigurationSettings();
        }

        public void SendEmail(
            string p_To,
            string p_CC,
            string p_BCC,
            string p_Subject,
            string p_TemplatePath,
            Dictionary<string, string> p_TemplateKeyValues)
        {
            string l_BodyEmailMessage = this.BuildEmailTemplate(p_TemplatePath, p_TemplateKeyValues);

            using (MailMessage l_EmailMessage = new MailMessage(this.SmtpSettings_DefaultFromAddress, p_To))
            {
                l_EmailMessage.Subject = p_Subject;
                l_EmailMessage.Body = l_BodyEmailMessage;

                l_EmailMessage.IsBodyHtml = true;

                this.SendEmail(l_EmailMessage);
            }
        }

        public void SendEmail(MailMessage p_EmailMessage)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = this.SmtpSettings_Server;
            smtpClient.EnableSsl = this.SmtpSettings_UseSSL;

            NetworkCredential networkCredentials = new NetworkCredential(
                this.SmtpSettings_Username, this.SmtpSettings_Password);

            smtpClient.UseDefaultCredentials = this.SmtpSettings_DefaultCredentials;
            smtpClient.Credentials = networkCredentials;
            smtpClient.Port = this.SmtpSettings_Port;
            smtpClient.DeliveryFormat = this.SmtpSettings_DeliveryFormat;
            smtpClient.DeliveryMethod = this.SmtpSettings_DeliveryMethod;

            this.SendEmail(smtpClient, p_EmailMessage);
        }

        public void SendEmail(SmtpClient p_SmtpClient, MailMessage p_EmailMessage)
        {
            p_SmtpClient.Send(p_EmailMessage);
        }

        private bool LoadSmtpSettingsFromConfigurationSettings()
        {
            bool l_SmtpSettingsLoadedSuccessfully = false;

            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            SmtpSettings_Server = smtpSection.Network.Host;
            SmtpSettings_Port = smtpSection.Network.Port;
            SmtpSettings_UseSSL = smtpSection.Network.EnableSsl;
            SmtpSettings_Username = smtpSection.Network.UserName;
            SmtpSettings_Password = smtpSection.Network.Password;
            SmtpSettings_DeliveryMethod = smtpSection.DeliveryMethod;
            SmtpSettings_DeliveryFormat = smtpSection.DeliveryFormat;
            SmtpSettings_DefaultCredentials = smtpSection.Network.DefaultCredentials;
            SmtpSettings_DefaultFromAddress = smtpSection.From;

            if (string.IsNullOrEmpty(SmtpSettings_Server))
            {
                this.LoadSmtpSettingsFromConfigurationAppSettings();
            }

            l_SmtpSettingsLoadedSuccessfully = true;

            return l_SmtpSettingsLoadedSuccessfully;
        }

        private bool LoadSmtpSettingsFromConfigurationAppSettings()
        {
            bool l_SmtpSettingsLoadedSuccessfully = false;

            if (string.IsNullOrEmpty(this.SmtpSettings_Server))
            {
                this.SmtpSettings_Server = ConfigurationManager.AppSettings[c_Key_SmtpSettings_Server];

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_Port]))
                {
                    this.SmtpSettings_Port = int.Parse(ConfigurationManager.AppSettings[c_Key_SmtpSettings_Port]);
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_UseSSL]))
                {
                    this.SmtpSettings_UseSSL = bool.Parse(ConfigurationManager.AppSettings[c_Key_SmtpSettings_UseSSL]);
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_Username]))
                {
                    this.SmtpSettings_Username = ConfigurationManager.AppSettings[c_Key_SmtpSettings_Username];
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_Password]))
                {
                    this.SmtpSettings_Password = ConfigurationManager.AppSettings[c_Key_SmtpSettings_Password];
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_DeliveryMethod]))
                {
                    this.SmtpSettings_DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), ConfigurationManager.AppSettings[c_Key_SmtpSettings_DeliveryMethod]);
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_DeliveryFormat]))
                {
                    this.SmtpSettings_DeliveryFormat = (SmtpDeliveryFormat)Enum.Parse(typeof(SmtpDeliveryFormat), ConfigurationManager.AppSettings[c_Key_SmtpSettings_DeliveryFormat]);
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_DefaultCredentials]))
                {
                    this.SmtpSettings_DefaultCredentials = bool.Parse(ConfigurationManager.AppSettings[c_Key_SmtpSettings_DefaultCredentials]);
                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[c_Key_SmtpSettings_DefaultFromAddress]))
                {
                    this.SmtpSettings_DefaultFromAddress = ConfigurationManager.AppSettings[c_Key_SmtpSettings_DefaultFromAddress];
                }
            }

            return l_SmtpSettingsLoadedSuccessfully;
        }

        public string BuildEmailTemplate(string p_TemplateFilePath, Dictionary<string, string> p_TemplateKeyValues)
        {
            string l_EmailBodyMessage = string.Empty;

            if (File.Exists(p_TemplateFilePath))
            {
                string templateFileContent = File.ReadAllText(p_TemplateFilePath);

                l_EmailBodyMessage = this.BuildEmailTemplateUsingTemplateKeyValues(templateFileContent, p_TemplateKeyValues);
            }

            return l_EmailBodyMessage;
        }

        private Dictionary<string, string> LowercaseTemplateKeyValues(Dictionary<string, string> p_TemplateKeyValues)
        {
            Dictionary<string, string> l_LowcaseTemplateKeyValuesToReturn = new Dictionary<string, string>();

            foreach (var keyValue in p_TemplateKeyValues)
            {
                l_LowcaseTemplateKeyValuesToReturn.Add(keyValue.Key.ToLower(), keyValue.Value);
            }

            return l_LowcaseTemplateKeyValuesToReturn;
        }

        private string BuildEmailTemplateUsingTemplateKeyValues(string p_EmailBodyContent, Dictionary<string, string> p_TemplateKeyValues)
        {
            StringBuilder emailMessageBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(p_EmailBodyContent))
            {
                if (p_EmailBodyContent.IndexOf("{{") >= 0 &&
                    p_TemplateKeyValues != null &&
                    p_TemplateKeyValues.Count > 0)
                {
                    var l_LowerCaseTemplateKeyValues = LowercaseTemplateKeyValues(p_TemplateKeyValues);

                    int lastProcessedIndex = 0;
                    for (int i = 0; i < p_EmailBodyContent.Length; i++)
                    {
                        if (p_EmailBodyContent[i] == '{' &&
                            i + 1 < p_EmailBodyContent.Length &&
                            p_EmailBodyContent[i + 1] == '{')
                        {
                            string scannedTemplateSoFarToAdd = p_EmailBodyContent.Substring(lastProcessedIndex, i - lastProcessedIndex);
                            emailMessageBuilder.Append(scannedTemplateSoFarToAdd);

                            lastProcessedIndex = i;

                            string CurrentKey = p_EmailBodyContent.Substring(i + 2, p_EmailBodyContent.IndexOf("}}", i + 2) - i - 2);
                            CurrentKey = CurrentKey.Trim().ToLower();
                            string CurrentValue = string.Empty;

                            if (l_LowerCaseTemplateKeyValues.TryGetValue(CurrentKey, out CurrentValue))
                            {
                                emailMessageBuilder.Append(CurrentValue);
                                lastProcessedIndex = p_EmailBodyContent.IndexOf("}}", i) + 2;
                            }
                            else
                            {
                                // Failed to find email template key value. Log exception.
                            }
                        }
                    }

                    if (lastProcessedIndex < p_EmailBodyContent.Length - 1)
                    {
                        emailMessageBuilder.Append(p_EmailBodyContent.Substring(lastProcessedIndex, p_EmailBodyContent.Length - lastProcessedIndex));
                        lastProcessedIndex = p_EmailBodyContent.Length - 1;
                    }
                }
                else
                {
                    return p_EmailBodyContent;
                }
            }

            return emailMessageBuilder.ToString();
        }
    }
}
