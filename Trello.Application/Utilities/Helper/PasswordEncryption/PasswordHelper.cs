using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.Helper.PasswordEncryption
{
    public static class PasswordHelper
    {
        
        public static string GenerateSalt()
        {
            var buffer = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }

        public static string HashPasswordWithMD5(string password, string salt)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static string HashPasswordWithSalt(string password)
        {
            var salt = GenerateSalt();
            var hash = HashPasswordWithMD5(password, salt);
            return $"{salt}:{hash}";
        }

        public static bool VerifyPassword(string password, string storedHashWithSalt)
        {
            var parts = storedHashWithSalt.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException("Invalid stored hash format.");
            }
            var salt = parts[0];
            var storedHash = parts[1];
            var hash = HashPasswordWithMD5(password, salt);
            return hash == storedHash;
        }
    }
}
