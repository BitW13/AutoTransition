using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class TransportationTypes
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Вид перевозки")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 3)]
        public string TransportationType { get; set; }
    }
}