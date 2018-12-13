using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Неправильный формат")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Неправильный формат")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пороли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}