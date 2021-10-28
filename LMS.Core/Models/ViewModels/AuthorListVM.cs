using System.Collections.Generic;

namespace Lms.Core.Models.ViewModels
{
    public class AuthorListVM
    {
        public List<AuthorCreateVM> Authors { get; set; }

        public AuthorListVM()
        {
            Authors = new List<AuthorCreateVM>();
        }
    }
}
