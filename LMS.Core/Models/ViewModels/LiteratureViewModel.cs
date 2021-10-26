using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.ViewModels
{
    public class LiteratureViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }
        public ICollection<AuthorViewModel> Authors { get; set; }

        public LiteratureViewModel()
        {
            Authors = new List<AuthorViewModel>();
        }
    }
}
