﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.ViewModels
{
    public class LiteratureAuthorCreateVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; }
    }
}
