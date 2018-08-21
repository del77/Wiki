using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class SuggestionDto
    {
        public int Id { get; set; }
        public UserDto Author { get; set; }
        public TextDetailsDto Text { get; set; }
        public string Content { get; set; }
        public int Served { get; set; }
    }
}
