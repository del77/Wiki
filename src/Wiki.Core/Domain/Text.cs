using System;

namespace Wiki.Core.Domain
{
    public class Text
    {
        public string Content { get; protected set; }

        public Status Status { get; protected set; }

        public int AuthorId {get; protected set; }
    }

    public enum Status { a, b, c }
}
