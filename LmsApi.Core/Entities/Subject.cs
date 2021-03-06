using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsApi.Core.Entities
{
    // Literature subject
    public class Subject : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        // NAV PROPERTIES
        public ICollection<Literature> Literatures { get; set; }
    }
}
