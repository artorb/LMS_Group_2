using System;

namespace Lms.Core.Entities
{
    public class Document :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; } 
        public string HashName { get; set; } //Path, file directory for ex.

        public string Uploader { get; set; } // Who uploaded the file

        public string ApplicationUserId { get; set; } // A file attached to a user account 
        public ApplicationUser ApplicationUser { get; set; } //I upload for me or for the teacher
        
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        
        public int? ModuleId { get; set; }
        public Module Module { get; set; }
        
        public int? ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}