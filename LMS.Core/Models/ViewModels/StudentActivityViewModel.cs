using Lms.Core.Entities;
using Lms.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class StudentActivityViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Please enter name!")]
        [MinLength(2, ErrorMessage = "Minimum length: 2 character!")]
        [MaxLength(40, ErrorMessage = "Maximum length: 40 character!")]
        [Display(Name = "Activity name")]
        public string ActivityName { get; set; }

        [Required(ErrorMessage = "Please enter description!")]
        [MinLength(5, ErrorMessage = "Minimum length: 5 character!")]
        [MaxLength(500, ErrorMessage = "Maximum length: 500 character!")]
        [Display(Name = "Description")]
        public string ActivityDescription { get; set; }

        [Required(ErrorMessage = "Please enter start date!")]   
        [AcitivtyStartTimeCheck]
        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime ActivityStartDate { get; set; }

        [Required(ErrorMessage = "Please enter end date!")]
        [ActivityEndTimeCheck]
        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime ActivityEndDate { get; set; }

        [ActivityEndTimeCheck]
        [Display(Name = "Deadline")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime? ActivityDeadline { get; set; }


        public string Status { get; set; }
    

        [Display(Name = "Activity documents")]
        public IEnumerable<Document> Documents { get; set; }

        public int ActivityTypeId { get; set; }
        public ActivityType ActivityTypes { get; set; }
    }
}
