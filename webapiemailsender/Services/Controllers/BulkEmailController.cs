using ClosedXML.Excel;
using webapiemailsender.Services.Data;
using webapiemailsender.Services.Helper;
using Microsoft.AspNetCore.Mvc;
using webapiemailsender.Services.Interfaces;

namespace webapiemailsender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkEmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly EmailSenderDbContext dbContext;

        public BulkEmailController(IEmailService service, EmailSenderDbContext dbContext)
        {
            this.emailService = service;
            this.dbContext = dbContext;
        }
        [HttpPost("SendBulkEmail")]
        public async Task<IActionResult> SendBulkEmail()
        {
            try
            {
                var emailAddresses = dbContext.Emails.Select(e => e.EmailAddress).ToList();

                if (emailAddresses.Count == 0)
                {
                    return BadRequest("Veritabanında hiç e-posta adresi bulunamadı.");
                }

                foreach (var emailAddress in emailAddresses)
                {
                    MailRequest mailrequest = new MailRequest();
                    mailrequest.ToEmail = emailAddress;
                    mailrequest.Subject = "Email Sender Web Api Toplu Mesaj Gönderimi Denemesi!";
                    mailrequest.Body = GetHtmlcontent();

                    await emailService.SendEmailAsync(mailrequest);
                }

                return Ok("Toplu e-posta başarıyla gönderildi.");
            }
            catch (Exception ex)
            {
                return BadRequest("Toplu e-posta gönderirken bir hata oluştu: " + ex.Message);
            }
        }
        private string GetHtmlcontent()
        {
            string Response = "<body style=\"margin: 0; padding: 0; box-sizing: border-box;\">\n";
            Response += "<div style=\"width: 100%;padding: 1rem; align-items: center; display: flex; flex-direction: column; background-color: #252525; color: #fff;\">";
            Response += " <h1>Email Api Test</h1>";
            Response += "<span>Denemedenemedeneme</span>";
            Response += "<div style=\"display: flex; gap: .5rem;\">";
            Response += "<span>Daha Fazlası İçin</span><a style=\"color: #fff; text-decoration: underline;\" href=\"https://www.instagram.com/berkayhuz\">Tıklayın</a>";
            Response += "</div>";
            Response += "</div>";
            Response += "</body>";
            return Response;
        }
    }
}