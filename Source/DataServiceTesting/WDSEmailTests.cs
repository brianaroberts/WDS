using DataService.Core.Data;
using DataService.Models;
using DataService.Models.Settings;
using System.Data;
using System.Net.Mail;
using WDS.Data;
using static DataService.Controllers.EmailController;

namespace DataServiceTesting
{
    public class WDSEmailTests
    {
        MailMessage _mailMessage = new MailMessage();
        EmailModel _emailModel = new EmailModel();

        [SetUp]
        public void Setup()
        {
            _mailMessage.Bcc.Add(new MailAddress("brianaroberts@live.com"));
            _mailMessage.Sender = new MailAddress("brianaroberts@live.com");
            _mailMessage.From = new MailAddress("brianaroberts@live.com");
            _mailMessage.Subject = "TEST TEST TEST";
            _mailMessage.Body = "TEST TEST TEST";
            _mailMessage.CC.Add(new MailAddress("brianaroberts@live.com"));

            
            _emailModel.From = "brianaroberts@live.com";
            _emailModel.Subject = "TEST TEST TEST";
            _emailModel.Message = "TEST TEST TEST";
            _emailModel.CC  = "brianaroberts@live.com";
            _emailModel.To = "brianaroberts@live.com";
        }

        [Test]
        public void TestSend()
        {
            try
            {
            WDSEmail.Send(_mailMessage);
                Assert.Pass();

            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task TestSendAsync()
        {
            try
            {
                await WDSEmail.SendAsync(_mailMessage);
                Assert.Pass();

            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestSendWithTemplate()
        {
            try
            {
                WDSEmail.SendWithTemplate(_emailModel);
                Assert.Pass();

            }
            catch
            {
                Assert.Fail();
            }
        }


        [Test]
        public async Task TestSendWithTemplateAsync()
        {
            try
            {
                await WDSEmail.SendWithTemplateAsync(_emailModel);
                Assert.Pass();

            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}