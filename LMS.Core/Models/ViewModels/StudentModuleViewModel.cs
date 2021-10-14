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
        [Display(Name = "Information:")] public string Information { get; set; }

        [Display(Name = "Activity:")] public Activity Activity { get; set; }

        [Display(Name = "The module started:")]
        public DateTime StartDate { get; set; }

        [Display(Name = "The module ends:")] public DateTime EndDate { get; set; }

        [Display(Name = "Documents")] public ICollection<Document> Documents { get; set; }
    }
}