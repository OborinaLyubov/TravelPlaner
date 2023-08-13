using System.ComponentModel.DataAnnotations;

namespace TravelPlanerLib.Models
{
    /// <summary>
    /// Модель для хранения данных по регистрируемому пользователю
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [Required]
        [Display(Name = "ФИО пользователя")]
        public string Name { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Display(Name = "Email пользователя")]
        public string Email { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        /// <summary>
        /// Повторный ввод пароля
        /// </summary>
        [Required]
        [Display(Name = "Повторите пароль")]
        public string PasswordConfirm { get; set; }
    }
}
