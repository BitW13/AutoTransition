using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class EditUserViewModel
    {
        public Guid Id { get; set; }

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
    }
}