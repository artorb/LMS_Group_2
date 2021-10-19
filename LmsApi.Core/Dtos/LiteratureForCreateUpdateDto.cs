using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApi.Core.Dtos
{
    public class LiteratureForCreateUpdateDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Url { get; set; }
        public int SubjectId { get; set; }
        public int LevelId { get; set; }

        public ICollection<AuthorDto> Authors { get; set; }
    }
}
