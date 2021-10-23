using System;
using System.Collections.Generic;

namespace LmsApi.Core.Dtos
{
    public class LiteratureDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }


        public List<AuthorDto> Authors { get; set; }
    }
}
