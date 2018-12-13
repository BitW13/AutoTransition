using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid AutoRouteId { get; set; }

        public DateTime LoadDate { get; set; }

        public DateTime UnloadDate { get; set; }

        public Guid TransportationTypeId { get; set; }

        public Guid CargoDimensionsId { get; set; }

        public double Weight { get; set; }

        public double Price { get; set; }

        public string Status { get; set; }
    }
}