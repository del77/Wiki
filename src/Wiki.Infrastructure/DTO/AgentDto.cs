using System;
using System.Collections.Generic;

namespace Wiki.Infrastructure.DTO
{
    public class AgentDto
    {
        public int UserId { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
