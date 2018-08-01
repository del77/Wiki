using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Core.Domain
{
    public class Suggestion
    {
        public int Id { get; protected set; }
        public string Content { get; protected set; }
    }
}
