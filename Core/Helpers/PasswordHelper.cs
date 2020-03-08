using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers
{
    public class PasswordHelper
    {
        private const string Salt = "myphunguyen89";
        public static string GenerateHash(string password)
        {
            var saltByte = Encoding.UTF8.GetBytes(Salt);
            var passwordByte = Encoding.UTF8.GetBytes(password);
            var hmacMD5 = new HMACMD5(saltByte);
            var hashedPassword = hmacMD5.ComputeHash(passwordByte);
            return BitConverter.ToString(hashedPassword).Replace("-", "").ToLower();
        }
    }
}
