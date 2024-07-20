using System.Net.Mail;
using System.Net;

namespace RingoMediaAssignment.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(string smtpServer, int port, string username, string password)
        {
            _smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@example.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
