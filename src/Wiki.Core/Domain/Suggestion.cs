using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Core.Domain
{
    public class Suggestion
    {
        public Suggestion(int? authorId, int? textId, string content)
        {
            AuthorId = authorId;
            TextId = textId;
            Content = content;
        }

        public int Id { get; protected set; }
        public int? AuthorId { get; protected set; }
        public int? TextId { get; protected set; }
        public string Content { get; protected set; }
    }
}
