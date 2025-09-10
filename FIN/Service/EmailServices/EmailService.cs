using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace FIN.Service.EmailServices
{
    public class EmailService
    {
        private readonly string smtpServer = "smtp.gmail.com"; // e.g., smtp.gmail.com
        private readonly int smtpPort = 587; // TLS port
        private readonly string smtpUser = "testmaila67@gmail.com";
        private readonly string smtpPass = "ccyd lrnw gofv qprg";

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("testmaila67@gmail.com", smtpUser));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpUser, smtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
