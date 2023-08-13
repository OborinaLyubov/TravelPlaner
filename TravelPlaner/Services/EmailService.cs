using MailKit.Net.Smtp;
using MimeKit;

namespace TravelPlaner.Services
{
    /// <summary>
    /// Сервис для отправки уведомлений через почту (по протоколу: smtp)
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Отправка уведомлений пользователям
        /// </summary>
        /// <param name="email">email пользователя</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="message">Тело сообщения</param>
        /// <returns></returns>
        public void SendEmail(string email, string subject, string message)
        {
            var apiKeys = new APIKeys();
            var messageMime = new MimeMessage();
            messageMime.From.Add(new MailboxAddress("Администратор", apiKeys.administratorEmail));
            messageMime.To.Add(new MailboxAddress("user", email));
            messageMime.Subject = subject;
            messageMime.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            try
            {
                var client = new SmtpClient();
                client.Connect("smtp.yandex.ru", 465, true);
                client.Authenticate(apiKeys.administratorLoginEmail, apiKeys.administratorPasswordEmail);
                client.Send(messageMime);
                client.Disconnect(true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
