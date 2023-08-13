namespace TravelPlaner
{
    /// <summary>
    /// Константы приложения
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Тело уведомления о завершении регистрации
        /// </summary>
        public string EndRegistrationNotificationBody = "Для завершения регистрации и подтверждения email перейдите по ссылке";
        /// <summary>
        /// Тема уведомления о завершении регистрации
        /// </summary>
        public string EndRegistrationNotificationSubject = "Завершение регистрации на сайте travel-planer.ru";
        /// <summary>
        /// Ошибка о заполнении обязательных полей при регистрации
        /// </summary>
        public string RequiredFieldsError = "Заполните обязательные поля";
        /// <summary>
        /// Ошибка о заполнении обязательных полей при авторизации
        /// </summary>
        public string LoginOrPasswordNotFound = "Email и/или пароль не установлены";
        /// <summary>
        /// Ошибка о существующем пользователе с указанными учетными данными
        /// </summary>
        public string ExistingUserForEmailOrLoginError = "С данным email и/или логином пользователь уже существует";
        /// <summary>
        /// Ошибка - несовпадение паролей при регистрации пользователя
        /// </summary>
        public string PasswordNotCoincidedError = "Пароли не совпадают";
    }
}
