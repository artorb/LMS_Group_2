using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
    public class ActivitySchemaViewModel
    {
        public string Name { get; set; }
        public string ActivityTypeName { get; set; }
        public bool DeadLine { get; set; }
        public bool Submitted { get; set; }
    }
}
