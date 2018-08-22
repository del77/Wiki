using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Agent
    {
        //private ISet<AgentPermission> permissions = new HashSet<AgentPermission>();
        public int UserId { get; protected set; }
        

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
