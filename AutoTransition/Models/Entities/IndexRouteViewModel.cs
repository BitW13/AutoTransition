using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class IndexRouteViewModel
    {
        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        public string StartCity { get; set; }

        [Display(Name = "Город загрузки")]
        [DataType(DataType.Text)]
        public string EndCity { get; set; }

        [Display(Name = "Расстояние")]
        [DataType(DataType.Text)]
        public double Distance { get; set; }
    }
}