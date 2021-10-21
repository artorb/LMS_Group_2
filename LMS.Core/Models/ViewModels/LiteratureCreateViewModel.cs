using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.ViewModels
{
    public class LiteratureCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name ="Subject")]
        public int SubjectId { get; set; }

        [Display(Name = "Level")]
        public int LevelId { get; set; }

        public ICollection<LiteratureAuthorCreateVM> Authors { get; set; } = new List<LiteratureAuthorCreateVM>();
    }
}
