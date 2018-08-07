using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class FilterInfo
    {
        public IEnumerable<ArticleCategoryDto> Categories { get; set; }
        public IEnumerable<TextTagDto> Tags { get; set; }
        public IEnumerable<TextStatusDto> Statuses { get; set; }
    }
}
