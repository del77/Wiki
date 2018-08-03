using System;
using System.Collections.Generic;
using System.Text;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(int userId, string role);
    }
}
