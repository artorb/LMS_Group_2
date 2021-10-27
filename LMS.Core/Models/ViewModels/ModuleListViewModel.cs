using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class ModuleListViewModel
    {
        public List<CreateModuleViewModel> Modules { get; set; }

        public ModuleListViewModel()
        {
            Modules = new List<CreateModuleViewModel>();
        }

        public ActivityListViewModel activityListViewModel { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
