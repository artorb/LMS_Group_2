using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Models.ViewModels
{
    public class LiteratureEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }


        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!(value.ToLower().StartsWith("http://")) || !(value.ToLower().StartsWith("https://")))
                        value = $"https://{value}";
                }
                url = value;
            }
        }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Display(Name = "Level")]
        public int LevelId { get; set; }
    }
}
