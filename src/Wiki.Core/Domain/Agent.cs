using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class Agent
    {
        private ISet<Permission> permissions = new HashSet<Permission>();
        public Guid UserId { get; protected set; }
        public IEnumerable<Permission> Permissions
        {
            get => permissions;
            set { permissions = new HashSet<Permission>(value); }
        } 

        public Agent(User user)
        {
            UserId = user.Id;
        }
        
    }

    public enum Permission { Modify, Accept, Publish, Write, Read };

}
