using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class UserClaims
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
    }
}