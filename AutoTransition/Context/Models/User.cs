﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Guid UserClaimsId { get; set; }

        public string UserRole { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public User()
        {
            Orders = new List<Order>();
        }
    }
}