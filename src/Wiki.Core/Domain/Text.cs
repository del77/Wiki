using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Text
    {
        // private IList<Suggestion> suggestions = new List<Suggestion>();
        private IList<string> suggestions = new List<string>();
        private ISet<string> tags = new HashSet<string>();

        public int Id { get; set; }
        public int ArticleId { get;  set; }
        public string Title { get;  set; }
        public int AuthorId { get;  set; }
        public string Version { get; set; }

        public IEnumerable<string> Suggestions
        {
            get => suggestions;
            protected set { }
        }

        public IEnumerable<string> Tags
        {
            get { return tags; }
            set { tags = new HashSet<string>(value); }
        }

        public string Content { get;  set; }

        // public Status Status { get; protected set; }
        public string Status { get;  set; }
    }

    public enum Status { a, b, c }
}
