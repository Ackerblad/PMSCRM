using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace PMSCRM.Services
{
    //public class EmailService
    //{
    //    private readonly IConfiguration _configuration;

    //    public EmailService(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public async Task SendEmail(string toEmail, string subject, string message)
    //    {
    //        var emailSettings = _configuration.GetSection("EmailSettings");

    //        var email = new MimeMessage();
    //        email.From.Add(MailboxAddress.Parse(emailSettings["SenderEmail"]));
    //        email.To.Add(MailboxAddress.Parse(toEmail));
    //        email.Subject = subject;

    //        email.Body = new TextPart("plain")
    //        {
    //            Text = message
    //        };

    //        using (var smtp = new SmtpClient())
    //        {
    //            await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), false);
    //            await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
    //            await smtp.SendAsync(email);
    //            await smtp.DisconnectAsync(true);
    //        }
    //    }
    //}
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
                //// This enables TLS encryption over an insecure connection.
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Use SecureSocketOptions.StartTls to enable encryption
                await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), SecureSocketOptions.StartTls);

                // Authenticate using the provided credentials
                await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);

                // Send the email
                await smtp.SendAsync(email);

                // Disconnect cleanly
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
