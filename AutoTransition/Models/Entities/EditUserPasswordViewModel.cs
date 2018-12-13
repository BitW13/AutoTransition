using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class EditUserPasswordViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Старый пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Пороли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}