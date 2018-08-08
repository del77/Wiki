using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.Web.ViewModels
{
    public class Article
    {
        [Required]
        public string Title { get; set; }
        [Required, StringLength(100)]
        public string Content { get; set; }
        public CategoryFilter Category { get; set; }
        public List<TagFilter> Tags { get; set; }
        public string Status { get; set; }
    }
}
