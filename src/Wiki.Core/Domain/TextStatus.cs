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
            SetStatus(status);
            Id = id;
        }

        public TextStatus(int id)
        {
            Id = id;
        }

        protected TextStatus()
        {

        }

        private void SetStatus(string status)
        {
            if (String.IsNullOrWhiteSpace(status))
                throw new Exception("status can't be empty");
            Status = status;
        }

    }
}
