using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Text
    {
        // private IList<Suggestion> suggestions = new List<Suggestion>();
        private IList<Suggestion> suggestions = new List<Suggestion>();
        private ISet<TextTag> tags = new HashSet<TextTag>();

        public Text(string title, string content, double version)
        {
            Title = title;
            Content = content;
            Version = version;
        }

        protected Text()
        {

        }

        public void SetAuthor(User author)
        {
            Author = author;
        }

        public void SetTags(IEnumerable<TextTag> tags)
        {
            Tags = tags;
        }

        public void SetStatus(TextStatus status)
        {
            Status = status;
        }

        public int Id { get; set; }
        public int ArticleId { get;  set; }
        public string Title { get;  set; }
        public User Author { get;  set; }
        public double Version { get; set; }
        public string TextComment { get; set; }

        public IEnumerable<Suggestion> Suggestions
        {
            get => suggestions;
            protected set { }
        }

        public IEnumerable<TextTag> Tags
        {
            get { return tags; }
            set { tags = new HashSet<TextTag>(value); }
        }

        public string Content { get;  set; }

        // public Status Status { get; protected set; }
        public TextStatus Status { get;  set; }
    }

}
