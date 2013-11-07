using System;
using System.Security.Cryptography;

namespace metrics.Support
{
    /// <summary>
    /// Provides statistically relevant random number generation
    /// </summary>
    public class Random
    {
        private static readonly RandomNumberGenerator _random;
        private static readonly System.Random _prng;

        public static bool UseCrypto { get; set; }

        static Random()
        {
            _random = RandomNumberGenerator.Create();
            _prng = new System.Random();
            UseCrypto = true;
        }

        public static long NextLong()
        {
            return (UseCrypto ? NextLongCrypto() : NextLongFast());
        }

        public static long NextLongCrypto()
        {
            var buffer = new byte[sizeof(long)];
            _random.GetBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long NextLongFast()
        {
            var buffer = new byte[sizeof(long)];
            _prng.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
