using System;
using System.Collections.Generic;

namespace Wiki.Core.Domain
{
    public class User
    {
        private ISet<UserPermission> permissions = new HashSet<UserPermission>();

        public IEnumerable<UserPermission> Permissions
        {
            get => permissions;
            set { permissions = new HashSet<UserPermission>(value); }
        }
        public int Id { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set;}
        public string Salt { get; protected set;}

        protected User()
        {}

        public User(string email, string password, string salt)
        {
            //Id = id;
            Email = email;
            Password = password;
            Salt = salt;
            
        }

        public void SetPermissions(ISet<UserPermission> permissions)
        {
            Permissions = permissions;
        }

        public User(int author)
        {
            Id = author;
        }
    }
}
