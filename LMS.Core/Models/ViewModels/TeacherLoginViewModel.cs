using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class TeacherLoginViewModel
    {
    
        public Course Course { get; set; }
        public string ActiveModuleName { get; set; }
        public string NextModuleName { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}
