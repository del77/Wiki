using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Core.Domain
{
    public class Suggestion
    {
        protected Suggestion() { }
        public Suggestion(string content, int served)
        {
            Content = content;
            Served = served;
        }

        public void SetAuthor(User author)
        {
            Author = author;
        }

        public void SetText(Text text)
        {
            Text = text;
        }

        public int Id { get; protected set; }
        public User Author{ get; protected set; }
        public Text Text { get; protected set; }
        public string Content { get; protected set; }
        public int Served { get; protected set; }

        public void MakeServed()
        {
            Served = 1;
        }
    }
}
