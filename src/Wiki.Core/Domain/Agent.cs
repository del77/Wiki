using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Agent
    {
        //private ISet<AgentPermission> permissions = new HashSet<AgentPermission>();
        private ISet<string> permissions = new HashSet<string>();
        public int UserId { get; protected set; }

        public IEnumerable<string> Permissions
        {
            get => permissions;
            set { permissions = new HashSet<string>(value); }
        }

        // public IEnumerable<AgentPermission> Permissions
        // {
        //     get => permissions;
        //     set { permissions = new HashSet<AgentPermission>(value); }
        // } 

        protected Agent() {}
        
        public Agent(User user)
        {
            UserId = user.Id;
        }
        
    }

    

}
