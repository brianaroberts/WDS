using WDS.Core.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using static DataService.Controllers.EmailController;
using DataService.Core.Settings;

namespace DataService.Core.Data
{
    public static class WDSEmail
    {
        public class EmailTemplate
        {
            [JsonProperty("to")]
            public string To;
            [JsonProperty("from")]
            public string From;
            [JsonProperty("cc")]
            public string CC;
            [JsonProperty("subject")]
            public string Subject;
            [JsonProperty("message")]
            public string Message;
        }

        private static SmtpClient _smtpClient = new SmtpClient();
        private static string _replyEmail = "WDS_NoReply@live.com";

        public static int Port
        {
            set
            {
                _smtpClient.Port = value;
            }
        }

        public static string Host
        {
            set
            {
                _smtpClient.Host = value;
            }
        }

        public static bool EnableSsl
        {
            set
            {
                _smtpClient.EnableSsl = value;
            }
        }

        static WDSEmail()
        {
            Host = "22.81.27.20";
            Port = 25; 
        }

        public static void Send(MailMessage emailMessage)
        {
            if (emailMessage.From == null)
                emailMessage.From = new MailAddress(_replyEmail); 

            _smtpClient.Send(emailMessage); 
        }

        public static async Task SendAsync(MailMessage emailMessage)
        {
            Send(emailMessage); 
        }

        public static void SendWithTemplate(EmailModel emailObj)
        {
            var templateLocation = $"{DataServiceSettings.EmailTemplatesDirectory}\\{emailObj.TemplateId}.json";
            var template = JsonConvert.DeserializeObject<EmailTemplate>(File.ReadAllText(templateLocation));
            if (emailObj.To == null && template.To.IsNullOrEmpty()) return; // No To, we are done!
            var emailMessage = new MailMessage();
            emailMessage.To.Add(emailObj.To);

            if (!template.To.IsNullOrEmpty())
                foreach (var addr in template.To.Split(";"))
                    emailMessage.To.Add(new MailAddress(addr));

            if (!emailObj.From.IsNullOrEmpty())
                emailMessage.From = new MailAddress(emailObj.From);
            else if (template.From != null)
                emailMessage.From = new MailAddress(template.From);
            else
                emailMessage.From = new MailAddress(_replyEmail);

            if (!template.CC.IsNullOrEmpty())
                foreach (var addr in template.CC.Split(";"))
                    emailMessage.CC.Add(new MailAddress(addr));

            if(emailMessage.Subject.IsNullOrEmpty())
            {
                if (template.Subject.IsNullOrEmpty()) return; // We require a subject here!
                emailMessage.Subject = template.Subject.ReplaceWith(emailObj.Parameters);
            }

            emailMessage.Body = template.Message.ReplaceWith(emailObj.Parameters);
            _smtpClient.Send(emailMessage);
        }

        public static async Task SendWithTemplateAsync(EmailModel emailObj)
        {
            SendWithTemplate(emailObj);
        }
    }
}
