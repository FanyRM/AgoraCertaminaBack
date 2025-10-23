using System.Security.Cryptography;

namespace AgoraCertaminaBack.Authorization.Utils
{
    public static class PasswordGenerator
    {
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string Digits = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        public static string Generate(int length = 12)
        {
            if (length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            string allCharacters = UppercaseLetters + LowercaseLetters + Digits + SpecialCharacters;
            char[] password = new char[length];
            var rng = RandomNumberGenerator.Create();

            // Ensure at least one character of each required type
            password[0] = GetRandomChar(UppercaseLetters, rng);
            password[1] = GetRandomChar(LowercaseLetters, rng);
            password[2] = GetRandomChar(Digits, rng);
            password[3] = GetRandomChar(SpecialCharacters, rng);

            for (int i = 4; i < length; i++)
            {
                password[i] = GetRandomChar(allCharacters, rng);
            }

            // Shuffle to avoid predictable order
            return new string(password.OrderBy(_ => Guid.NewGuid()).ToArray());
        }

        private static char GetRandomChar(string characters, RandomNumberGenerator rng)
        {
            byte[] data = new byte[1];
            int maxValue = byte.MaxValue - byte.MaxValue % characters.Length;

            do
            {
                rng.GetBytes(data);
            } while (data[0] >= maxValue);

            return characters[data[0] % characters.Length];
        }
    }
}
