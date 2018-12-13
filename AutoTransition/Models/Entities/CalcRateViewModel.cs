using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class CalcRateViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        public string StartCity { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        public string EndCity { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Место загрузки")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 10)]
        public string StartAddress { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Место разгрузки")]
        [DataType(DataType.Text)]
        [StringLength(200, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 10)]
        public string EndAddress { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Дата загрузки")]
        [DataType(DataType.Date)]
        public DateTime LoadDate { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Вид перевозки")]
        [DataType(DataType.Text)]
        public string TransportationType { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Габариты груза (Ш/Д/В, м)")]
        [DataType(DataType.Text)]
        public string CargoDimensions { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Вес, кг")]
        [DataType(DataType.Text)]
        public double Weight { get; set; }
    }
}