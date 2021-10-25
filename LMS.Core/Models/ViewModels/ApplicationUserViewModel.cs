using Lms.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
  public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }



        //Navigations:
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}
