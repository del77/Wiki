using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public long Expiry { get; set; }
    }
}
