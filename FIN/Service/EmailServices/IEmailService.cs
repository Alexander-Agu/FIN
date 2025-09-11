namespace FIN.Service.EmailServices
{
    public interface IEmailService
    {
        // Sends an email to users or admins
        public Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}
