using MimeKit;
using MailKit.Net.Smtp;

namespace PMSCRM.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string message)
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
                smtp.Connect(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), false);
                smtp.Authenticate(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
