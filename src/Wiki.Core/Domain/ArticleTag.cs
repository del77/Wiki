using System;

namespace Wiki.Core.Domain
{
    public class ArticleTag
    {
        public int ArticleId { get; protected set; }
        public Tag Tag { get; protected set; }
    }
    public enum Tag { a, b, c}
}
