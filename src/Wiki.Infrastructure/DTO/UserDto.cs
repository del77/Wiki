using System;
using System.Collections.Generic;

namespace Wiki.Infrastructure.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserPermissionDto> Permissions { get; set; }
    }
}
