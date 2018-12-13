using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class AutoRoute
    {
        public Guid Id { get; set; }

        public Guid StartAddressId { get; set; }

        public Guid EndAddressId { get; set; }

        public double Distance { get; set; }
    }
}