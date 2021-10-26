using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Models.ViewModels
{
    public class AuthorListVM
    {
        public List<LiteratureAuthorCreateVM> Authors { get; set; }

        public AuthorListVM()
        {
            Authors = new List<LiteratureAuthorCreateVM>();
        }
    }
}
