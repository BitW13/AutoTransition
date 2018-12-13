using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class AccountIndexViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Количество активных заказов")]
        [DataType(DataType.Text)]
        public int CountOfActiveOrders { get; set; }
    }
}