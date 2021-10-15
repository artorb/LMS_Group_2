using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Entities
{
    public class ActivityType : BaseEntity
    {
        [Display(Name = "Type")]
        public string TypeName { get; set; }
    }
}