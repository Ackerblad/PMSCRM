using MailKit.Net.Smtp;
using MimeKit;

namespace PMSCRM.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailSettings["SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), false);
                await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
