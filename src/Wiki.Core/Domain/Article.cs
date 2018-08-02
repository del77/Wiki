using System;
using System.Collections.Generic;
namespace Wiki.Core.Domain
{
    public class Article
    {
        private IDictionary<double, Text> texts = new Dictionary<double, Text>();
        //private ISet<ArticleTag> tags = new HashSet<ArticleTag>();
        private ISet<string> tags = new HashSet<string>();


        public int Id { get; protected set; }
        public string Title {get; protected set; }
        //public Category category {get; protected set;}
        public string Category {get; protected set;}
        public IDictionary<double, Text> Texts { get { return texts; }}
        // public IEnumerable<ArticleTag> Tags 
        // {
        //     get { return tags; }
        //     set { tags = new HashSet<ArticleTag>(value); }
        // }

        public IEnumerable<string> Tags 
        {
            get { return tags; }
            set { tags = new HashSet<string>(value); }
        }
    }

    public enum Category { a, b, c }

}
