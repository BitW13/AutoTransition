using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class OrderIndexViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        public string StartCity { get; set; }

        [Display(Name = "Адрес загрузки")]
        [DataType(DataType.Text)]
        public string StartAddressInCity { get; set; }

        [Display(Name = "Город разгрузки")]
        [DataType(DataType.Text)]
        public string EndCity { get; set; }

        [Display(Name = "Адрес разгрузки")]
        [DataType(DataType.Text)]
        public string EndAddressInCity { get; set; }

        [Display(Name = "Дата загрузки")]
        [DataType(DataType.Date)]
        public DateTime LoadDate { get; set; }

        [Display(Name = "Приблизительная дата разгрузки")]
        [DataType(DataType.Date)]
        public DateTime UnloadDate { get; set; }

        [Display(Name = "Вид транспорта")]
        [DataType(DataType.Text)]
        public string TransportationType { get; set; }

        [Display(Name = "Габариты груза")]
        [DataType(DataType.Text)]
        public string CargoDimensions { get; set; }

        [Display(Name = "Вес груза")]
        [DataType(DataType.Text)]
        public double Weight { get; set; }

        [Display(Name = "Стоимость")]
        [DataType(DataType.Text)]
        public double Price { get; set; }

        [Display(Name = "Статус заказа")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
    }
}