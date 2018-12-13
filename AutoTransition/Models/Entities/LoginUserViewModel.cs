using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Неправильный формат")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }
    }
}