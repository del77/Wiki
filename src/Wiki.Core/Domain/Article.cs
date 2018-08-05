using System;
using System.Collections.Generic;
namespace Wiki.Core.Domain
{
    public class Article
    {
        private IDictionary<double, Text> texts = new Dictionary<double, Text>();
        //private ISet<ArticleTag> tags = new HashSet<ArticleTag>();
        

        public int Id { get;  set; }
        
        public Text Master { get;  set; }
        //public Category category {get; protected set;}
        public string Category {get;  set;}
        public IDictionary<double, Text> Texts { get { return texts; }
            set { texts = value; }
        }
        // public IEnumerable<ArticleTag> Tags 
        // {
        //     get { return tags; }
        //     set { tags = new HashSet<ArticleTag>(value); }
        // }


    }

    public enum Category { a, b, c }

}
