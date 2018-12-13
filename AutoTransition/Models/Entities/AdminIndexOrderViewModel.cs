using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class AdminIndexOrderViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Дата загрузки")]
        [DataType(DataType.DateTime)]
        public DateTime LoadDate { get; set; }

        [Display(Name = "Дата разгрузки")]
        [DataType(DataType.DateTime)]
        public DateTime UnloadDate { get; set; }

        [Display(Name = "Цена")]
        [DataType(DataType.Text)]
        public double Price { get; set; }

        [Display(Name = "Статус")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
    }
}