using System;

namespace Wiki.Core.Domain
{
    public class User
    {
        public int Id { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set;}
        public string Salt { get; protected set;}

        protected User()
        {}

        public User(int id, string email, string password, string salt)
        {
            Id = id;
            Email = email;
            Password = password;
            Salt = salt;
            
        }
        
    }
}
