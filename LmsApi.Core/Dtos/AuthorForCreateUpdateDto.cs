using System;

namespace LmsApi.Core.Dtos
{
    public class AuthorForCreateUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
