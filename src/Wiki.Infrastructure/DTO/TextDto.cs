using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class TextDto
    {
        public string Title { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Status { get; set; }
    }
}
