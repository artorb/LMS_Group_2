using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.ViewModels
{
    public class AuthorViewModel
    {
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birthday")]
        public DateTime BirthDay { get; set; }
    }
}
