using System;
using System.Collections.Generic;

namespace Lms.Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public ICollection<Document> Documents { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Module> Modules { get; set; }

    }
}