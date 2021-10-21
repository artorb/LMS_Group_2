using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsApi.Core.Entities
{
    public class Author : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }

        // NAV PROPERTIES
        public ICollection<Literature> Literatures { get; set; }
    }
}
