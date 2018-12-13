using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class GetRightsUserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 5)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Защитный код")]
        [DataType(DataType.Password)]
        public string SecurityCode { get; set; }
    }
}