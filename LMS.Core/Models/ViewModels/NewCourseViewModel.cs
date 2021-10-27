using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lms.Core.Entities;

namespace Lms.Core.Models.ViewModels
{
    public class NewCourseViewModel
    {
        [DisplayName("Test")]
        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ModuleName { get; set; }

        public string ModuleDescription { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]  
        public DateTime ModuleStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime ModuleEndDate { get; set; }

        public ICollection<Document> Documents { get; set; }
        public ICollection<Module> Modules { get; set; }
    }
}