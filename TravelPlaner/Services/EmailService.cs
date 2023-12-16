using EmailSenderLib.Smtp;

namespace TravelPlaner.Services
{
    /// <summary>
    /// Сервис для отправки уведомлений через почту (по протоколу: smtp)
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Хост почтового сервера отправителя
        /// </summary>
        private const string Host = "smtp.yandex.ru";

        /// <summary>
        /// Отправка уведомлений пользователям
        /// </summary>
        /// <param name="email">email получателя</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="message">Тело сообщения</param>
        public void SendEmail(string email, string subject, string message)
        {
            var senderEmail = EmailSender.GetSenderAddress(Host);

            var mailMessage = EmailSender.AddMailMessage(senderEmail, email, subject, message);

            EmailSender.Send(mailMessage, Host);
        }
    }
}
