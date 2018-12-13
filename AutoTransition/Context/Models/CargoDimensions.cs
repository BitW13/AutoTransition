using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoTransition.Context.Models
{
    public class CargoDimensions
    {
        public Guid Id { get; set; }

        public double Length { get; set; }

        public double Width { get; set; }

        public double Hight { get; set; }
    }
}