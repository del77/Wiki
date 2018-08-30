using System;

namespace Wiki.Core.Domain
{
    public class TextTag
    {
        public int Id { get; protected set; }
        public int TextId { get; protected set; }
        public string Tag { get; protected set; }

        public TextTag(int id, string tag)
        {
            Id = id;
            Tag = tag;
        }

        public TextTag(string tag)
        {
            Tag = tag;
        }

        public TextTag(int id)
        {
            Id = id;
        }

        public TextTag(int id, int textId)
        {
            Id = id;
            TextId = textId;
        }

        protected TextTag()
        {

        }

        
    }


}
