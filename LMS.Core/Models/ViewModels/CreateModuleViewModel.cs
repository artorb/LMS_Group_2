using Lms.Core.Entities;
using Lms.Core.Validations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
    public class CreateModuleViewModel
    {
        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string ModuleName { get; set; }


        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string ModuleDescription { get; set; }


        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]
        //[ModuleStartTimeCheck]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Remote(action: "ModuleStartTimeCheck", controller: "Validations", AdditionalFields = nameof(CourseId))]
        public DateTime ModuleStartDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        // [ModuleEndTimeCheck]       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        //[Remote(action: "ModuleEndTimeCheck", controller: "Validations", AdditionalFields = nameof(CourseId))]
        public DateTime ModuleEndDate { get; set; }



        public int CourseId { get; set; }
        public Course Course { get; set; }
        //public List<Activity> Activities { get; set; }

        public ICollection<CreateActivityViewModel> Activities { get; set; }

        public CreateModuleViewModel()
        {
            Activities = new List<CreateActivityViewModel>();
        }

    }
}
