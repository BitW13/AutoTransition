﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutoTransition.Models.Entities
{
    public class EditRecordViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок")]
        [StringLength(600, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 10)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Описание")]
        [StringLength(10000, ErrorMessage = "Это поле должно быть от {2} до {1} символов", MinimumLength = 50)]
        public string Description { get; set; }
    }
}