using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.Web.ViewModels
{
    public class Author
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool Selected { get; set; }
    }
}
