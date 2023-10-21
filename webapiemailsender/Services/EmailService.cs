using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using webapiemailsender.Services.Interfaces;
using webapiemailsender.Services.Helper;

namespace webapiemailsender.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> options) { 
            this.emailSettings=options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject=mailrequest.Subject;
            var builder = new BodyBuilder();
          

            byte[] fileBytes; 
            if (System.IO.File.Exists("Assets/Attachment/test.pdf"))
            {
                FileStream file = new FileStream("Assets/Attachment/test.pdf", FileMode.Open, FileAccess.Read);
                using(var ms=new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes=ms.ToArray();
                }
                builder.Attachments.Add("test.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
                builder.Attachments.Add("test2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            }

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
