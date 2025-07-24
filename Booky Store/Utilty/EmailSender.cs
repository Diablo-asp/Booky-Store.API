using System.Net.Mail;
using System.Net;

namespace Booky_Store.API.Utilty
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("di0ab0lo0@Gmail.com", "kixf mzbp znqp womj")
            };

            return client.SendMailAsync(
                new MailMessage(from: "di0ab0lo0@Gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
