using System;
using System.Linq;
using Lms.Core.Entities;

namespace Lms.Core.Models.ViewModels
{
   public class TeacherTableViewModel
    {
        public Course Course { get; set; }
        public string ActiveModuleName { get; set; }
        public string NextModuleName { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}
