using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Core.Domain
{
    public class ArticleCategory
    {
        public int Id { get; protected set; }
        public string Category { get; protected set; }

        public ArticleCategory(int id, string category)
        {
            Id = id;
            Category = category;
        }

        public ArticleCategory(string category)
        {
            Category = category;
        }

        public ArticleCategory(int id)
        {
            Id = id;
        }

        protected ArticleCategory()
        {

        }
    }
}
