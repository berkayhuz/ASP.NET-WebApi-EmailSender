using webapiemailsender.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace webapiemailsender.Services.Data
{
    public class EmailSenderDbContext : DbContext
    {
        public EmailSenderDbContext(DbContextOptions<EmailSenderDbContext> options) : base(options) { }

        public DbSet<Email> Emails { get; set; }
    }
}
