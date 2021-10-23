using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsApi.Core.Entities
{
    public class Literature : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Url { get; set; }
        public int CategoryId { get; set; } // FK
        public int SubjectId { get; set; } // FK
        public int LevelId { get; set; } // FK

        // NAV PROPERTIES
        public ICollection<Author> Authors { get; set; }
        public Category Category { get; set; }
        public Subject Subject { get; set; }
        public Level Level { get; set; }
    }
}
