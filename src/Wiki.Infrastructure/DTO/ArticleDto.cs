using System;

namespace Wiki.Infrastructure.DTO
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title {get; set; }
        public string Category {get; set;}
    }
}
