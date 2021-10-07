using System;
using System.Collections;
using System.Collections.Generic;

namespace Lms.Core.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public ICollection<Activity> Activities { get; set; }
        
        public ICollection<Document> Documents { get; set; }
    }
}