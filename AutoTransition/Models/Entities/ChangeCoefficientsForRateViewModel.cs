using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class ChangeCoefficientsForRateViewModel
    {
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Коэффициент перевозки опасного груза")]
        [DataType(DataType.Text)]
        public double CoeffDangerousLoads { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Коэффициент перевозки с температурным режимом")]
        [DataType(DataType.Text)]
        public double CoeffRefrTransport { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Коэффициент перевозки сборного груза")]
        [DataType(DataType.Text)]
        public double CoeffGroupLoads { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Коэффициент перевозки комплектного груза")]
        [DataType(DataType.Text)]
        public double CoeffCompleteLoads { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Коэффициент дистанции")]
        [DataType(DataType.Text)]
        public double CoeffDistanceByHundred { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Коэффициент прибыли")]
        [DataType(DataType.Text)]
        public double CoeffProfit { get; set; }
    }
}