using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class Address
    {
        public Guid Id { get; set; }

        public string City { get; set; }

        public string AddressInCity { get; set; }
    }
}