using DataService.Core.Data;
using WDS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WDS.Authorization;
using DataService.Models;
using WDSClient.Models;
using DataService.Core.Settings;
using DataService.Core.Logging;
using static WDSClient.Models.WDSEnums;

namespace DataService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        
        public class EmailModel
        {
            [JsonProperty("templateid")]
            public string TemplateId;
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
            [JsonProperty("parameters")]
            public Dictionary<string, string> Parameters;
        }

        [HttpPost("EmailWithTemplate")]
        [Authorize(Policy = Policies.Read)]
        public IActionResult EmailWithTemplate()
        {
            Task<string> requestBody;
            WDSServerResponse response = new WDSServerResponse();
            response.StartTimer();
            
            try
            {
                using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestBody = sr.ReadToEndAsync();
                }

                List<EmailModel> emails = JsonConvert.DeserializeObject<List<EmailModel>>(requestBody.Result);

                foreach (var email in emails)
                {
                    //var mm = new MailMessage(email.From, email.To, email.Subject, email.Message); 
                    WDSEmail.SendWithTemplate(email); 
                }

                //var callList = emails.Select(
                //    c => WDSEmail.SendWithTemplateAsync(new MailMessage(c.From, c.To, c.Subject, c.Message),
                //        c.TemplateId, c.UserParameters)

                //    )
                //    .ToList();
                //Task.WhenAll(callList);
                response.ReturnValue = $"Succesfully processed {emails.Count} emails!";
            } catch (Exception ex)
            {
                response.ErrorMessage = $"Error occurred in WDSEmail.EmailWithTemplate() - {ex.Message}";
                WDSLogs.WriteServiceDBLog(LogLevels.Error, ex.Message); 
            }
            response.StopTimer();

            return new ObjectResult(response);
        }
    }
}
