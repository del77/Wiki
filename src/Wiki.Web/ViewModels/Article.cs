using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.Web.ViewModels
{
    public class Article
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public Filter Filter { get; set; }
    }
}
