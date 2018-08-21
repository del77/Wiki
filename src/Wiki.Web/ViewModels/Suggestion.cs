using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.Web.ViewModels
{
    public class Suggestion
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public Author Author { get; set; }
        public Article Article { get; set; }
        public int Status { get; set; }
    }
}
