using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
   public class ActivityListViewModel
    {
        public List<CreateActivityViewModel> Activities { get; set; }

        public ActivityListViewModel()
        {
            Activities = new List<CreateActivityViewModel>();
        }

        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}

