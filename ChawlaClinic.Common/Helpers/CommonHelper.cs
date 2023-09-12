using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ChawlaClinic.Common.Helpers
{
    public class CommonHelper
    {
        private static int HashSize = 32;
        private static int SaltSize = 16;
        public static (byte[], byte[]) PasswordHashSalt(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = GenerateHash(password, salt);

            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hash);
            return (hash, salt);
        }
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private static byte[] GenerateHash(string password, byte[] salt)
        {
            int iterations = 10000; // You can adjust this value for security
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, iterations, HashSize);
            return hash;
        }
    }
}
