using System;

namespace Wiki.Core.Domain
{
    public class UserPermission
    {
        public UserPermission(int id, int userId)
        {
            Id = id;
            UserId = userId;
        }

        protected UserPermission() { }
        
        public int Id { get; protected set; }
        public int UserId { get; protected set; }
        public string Permission { get; protected set; }
    }
}
