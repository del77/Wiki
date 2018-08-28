using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.Web.ViewModels
{
    public class Article
    {
        public int ArticleId { get; set; }
        public int TextId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public Author Author { get; set; }
        public User Supervisor { get; set; }
        public CategoryFilter Category { get; set; }
        public List<TagFilter> Tags { get; set; }
        public StatusFilter Status { get; set; }
        public double Version { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}