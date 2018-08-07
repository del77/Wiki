using System;
using System.Collections.Generic;

namespace Wiki.Infrastructure.DTO
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public ArticleCategoryDto Category {get; set; }
        public TextDto Master { get; set; }
        public IEnumerable<TextDto> Texts { get; set; }
    }
}
