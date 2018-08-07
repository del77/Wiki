using System;

namespace Wiki.Core.Domain
{
    public class TextTag
    {
        public int Id { get; protected set; }
        public string Tag { get; protected set; }

        public TextTag(int id, string tag)
        {
            Id = id;
            Tag = tag;
        }

        protected TextTag()
        {

        }

        
    }


}
