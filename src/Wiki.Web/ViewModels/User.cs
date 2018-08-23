using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wiki.Web.ViewModels
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Permission> Permissions { get; set; }
    }

    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public bool Checked { get; set; }
    }
}
