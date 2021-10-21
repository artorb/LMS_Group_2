using Lms.Core.Entities;
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

        [Display(Name = "Activity name")]
        public string ActivityName { get; set; }


        [Display(Name = "Description")]
        public string ActivityDescription { get; set; }


        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime ActivityStartDate { get; set; }


        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime ActivityEndDate { get; set; }


        public string Status { get; set; }
    

        [Display(Name = "Activity documents")]
        public IEnumerable<Document> Documents { get; set; }

        public int ActivityTypeId { get; set; }
        public ActivityType ActivityTypes { get; set; }
    }
}
