using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Core.Domain
{
    public class TextStatus
    {
        public int Id { get; protected set; }
        public string Status { get; protected set; }

        public TextStatus(int id, string status)
        {
            Id = id;
            Status = status;
        }

        protected TextStatus()
        {

        }

    }
}
