using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class Record
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime RecordDate { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}