using Lms.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class CreateActivityViewModel
    {
        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string ActivityName { get; set; }


        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string ActivityDescription { get; set; }
        

        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        // [AcitivtyStartTimeCheck]
        public DateTime ActivityStartDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        // [ActivityEndTimeCheck]
        public DateTime ActivityEndDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "Activity Deadline")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime ActivityDeadline { get; set; }


        [Required(ErrorMessage = "Please enter the type!")]
        public int ActivityTypeId { get; set; }
        public IEnumerable<SelectListItem> ActivityType { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}
