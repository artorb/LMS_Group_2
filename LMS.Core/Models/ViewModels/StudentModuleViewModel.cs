using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class StudentModuleViewModel
    {   
        [Display(Name = "Module name")]
        public string ModuleName { get; set; }

        [Display(Name = "Information")]
        public string ModuleDescription { get; set; }
        public DateTime ModuleStartDate { get; set; }
        public DateTime ModuleEndDate { get; set; }



        public IEnumerable<Activity> Activities { get; set; } 
        public IEnumerable<Document> Documents { get; set; }
        //public IEnumerable<ActivityType> ActivityTypes { get; set; }

    }
}
