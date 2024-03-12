using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ChawlaClinic.Common.Helpers
{
    public class CommonHelper
    {
        private static int HashSize = 32;
        private static int SaltSize = 32;

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

            return IntToBase36(randomValue).PadLeft(length, '0');
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

        public static string GenerateSecureCode(int value)
        {
            string base36AutoIncremented = IntToBase36(value);

            string randomBase36 = GenerateRandomBase36String(3);

            return base36AutoIncremented + randomBase36;
        }
    }
}
