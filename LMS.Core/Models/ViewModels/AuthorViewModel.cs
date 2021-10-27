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
        [Display(Name = "Birthdate")]
        public DateTime? BirthDate { get; set; }

        public int Age
        {
            get 
            {
                if (BirthDate == null) return 0;

                //var age = (DateTime.Now.Year - BirthDate.Value.Year);
                //if (DateTime.Now.Year - age < BirthDate.Value.Year) return age + 1;
                var age = (DateTime.Now.Year - BirthDate.Value.Year);
                if (DateTime.Now.AddYears(-age) < BirthDate.Value) return age - 1;
                return age;
            }
        }

    }
}
