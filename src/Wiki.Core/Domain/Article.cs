using System;
using System.Collections.Generic;
namespace Wiki.Core.Domain
{
    public class Article
    {
        private IDictionary<double, Text> texts = new Dictionary<double, Text>();
        private IList<Tag> tags = new List<Tag>();


        public Category category {get; protected set;}
        public IDictionary<double, Text> Texts { get { return texts; }}
        public IEnumerable<Tag> Tags 
        {
            get { return tags; }
            set { tags = new List<Tag>(value); }
        }
    }

    public enum Category { a, b, c }
    public enum Tag { a, b, c}
}
