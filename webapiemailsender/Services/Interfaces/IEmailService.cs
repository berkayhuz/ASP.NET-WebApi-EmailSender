using webapiemailsender.Services.Helper;

namespace webapiemailsender.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailrequest);
    }
}
