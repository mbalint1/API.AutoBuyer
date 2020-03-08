using System;
using System.Security.Cryptography;

namespace AutoBuyer.API.Core.Utilities
{
    public static class PasswordUtility
    {
        public static string HashPassword(string password)
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] salt;
                cryptoProvider.GetBytes(salt = new byte[16]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
                {
                    var hash = pbkdf2.GetBytes(20);
                    var hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public static bool VerfiyHash(string hashedPass, string userPass)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPass);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}