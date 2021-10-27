using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.ViewModels
{
    public class PersonnalListViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [Display(Name = "Role")]
        public List<string> RoleName { get; set; }
    }
}
