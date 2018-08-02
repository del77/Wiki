using System;

namespace Wiki.Core.Domain
{
    public class AgentPermission
    {
        public int AgentId { get; protected set; }
        public Permission Permission { get; protected set; }
    }

    public enum Permission { Modify, Accept, Publish, Write, Read };
}
