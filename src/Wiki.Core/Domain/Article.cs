using System;
using System.Collections.Generic;
namespace Wiki.Core.Domain
{
    public class Article
    {
        private IList<Text> texts = new List<Text>();


        public Article(int id)
        {
            Id = id;
        }

        protected Article()
        {
        }

        public void SetCategory(ArticleCategory category)
        {
            Category = category;
        }

        public void SetText(Text text)
        {
            Master = text;
        }


        public int Id { get;  set; }
        
        public Text Master { get;  set; }
        
        public ArticleCategory Category {get; protected set;}
        public IEnumerable<Text> Texts { get { return texts; }
            set { texts = new List<Text>(value); }
        }

        public void SetMaster(Text text)
        {
            Master = text;
        }
    }

}
