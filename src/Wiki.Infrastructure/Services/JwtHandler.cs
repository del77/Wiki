using System;
using System.Collections.Generic;
using System.Text;
using Wiki.Infrastructure.DTO;

namespace Wiki.Infrastructure.Services
{
    public class JwtHandler : IJwtHandler
    {
        public JwtDto CreateToken(int userId, string role)
        {
            throw new NotImplementedException();
        }
    }
}
