using System;
using System.Collections.Generic;
using System.Text;

namespace Wiki.Infrastructure.Services
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}
