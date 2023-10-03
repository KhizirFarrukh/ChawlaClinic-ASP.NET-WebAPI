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
        public static bool ValidatePassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            byte[] enteredHash = GenerateHash(enteredPassword, storedSalt);
            return enteredHash.SequenceEqual(storedHash);
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
            int iterations = 10000;
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, iterations, HashSize);
            return hash;
        }

        private static string GenerateRandomBase36String(int length)
        {
            Random random = new Random();

            int randomValue = random.Next((int)Math.Pow(36,length));

            string base36Value = IntToBase36(randomValue).PadLeft(length, '0');

            return base36Value;
        }

        private static string IntToBase36(long value)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;

            while (value > 0)
            {
                int remainder = (int)(value % 36);
                result = chars[remainder] + result;
                value /= 36;
            }

            return result;
        }

        public static string GenerateSecureToken(int value)
        {
            string base36AutoIncremented = IntToBase36(value).PadLeft(3, '0');

            string randomBase36 = GenerateRandomBase36String(3);

            string secureToken = base36AutoIncremented + randomBase36;

            return secureToken;
        }
    }
}
