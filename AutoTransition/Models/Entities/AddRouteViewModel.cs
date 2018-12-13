using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class AddRouteViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 2)]
        public string StartCity { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 2)]
        public string EndCity { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Расстояние")]
        [DataType(DataType.Text)]
        public double Distance { get; set; }
    }
}