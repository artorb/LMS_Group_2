using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
    public class DaySchemaViewModel
    {
        public string WeekDay { get; set; }
        public ICollection<ActivitySchemaViewModel> ActivitySchemas { get; set; } 
    }
}