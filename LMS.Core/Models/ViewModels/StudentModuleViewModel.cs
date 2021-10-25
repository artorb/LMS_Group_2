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
        public int Id { get; set; }


        [Display(Name = "Module name")]
        public string ModuleName { get; set; }


        [Display(Name = "Description")]
        public string ModuleDescription { get; set; }


        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]


        public DateTime ModuleStartDate { get; set; }


        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime ModuleEndDate { get; set; }


        public string Status { get; set; }


        public IEnumerable<Activity> Activities { get; set; }


        [Display(Name = "Module documents")]
        public IEnumerable<Document> Documents { get; set; }
    }
}