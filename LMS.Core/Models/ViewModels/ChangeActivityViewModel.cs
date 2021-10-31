using Lms.Core.Entities;
using Lms.Core.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class ChangeActivityViewModel
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Please enter start date!")]
        [Display(Name = "Start date")]    
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage = "Please enter end date!")]
        [Display(Name = "End date")]
        [ActivityEndTimeCheck]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }


        [ActivityEndTimeCheck]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? Deadline { get; set; }



        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Required(ErrorMessage = "Please choose type!")]
        public int ActivityTypeId { get; set; }
 

        public ICollection<Document> Documents { get; set; }

        public IEnumerable<SelectListItem> ActivityType { get; set; }
    }
}
