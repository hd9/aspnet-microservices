using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Microservices.Core.Infrastructure.Crypt
{
    public class Crypt
    {
        public static string HashPassword(string passwd, string salt)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.Unicode.GetBytes(passwd + salt));
                return Convert.ToBase64String(hash);
            }
        }

        public static string GenerateSalt()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
