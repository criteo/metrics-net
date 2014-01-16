using System;
using System.Security.Cryptography;
using System.Threading;

namespace metrics.Support
{
    /// <summary>
    /// Provides statistically relevant random number generation
    /// </summary>
    public class Random
    {
		private static readonly ThreadLocal<RandomNumberGenerator> _random = new ThreadLocal<RandomNumberGenerator>(RandomNumberGenerator.Create);
        private static readonly ThreadLocal<System.Random> _prng = new ThreadLocal<System.Random>(() => new System.Random());

        public static bool UseCrypto { get; set; }

        static Random()
        {
            UseCrypto = true;
        }

        public static long NextLong()
        {
            return (UseCrypto ? NextLongCrypto() : NextLongFast());
        }

        public static long NextLongCrypto()
        {
            var buffer = new byte[sizeof(long)];
            _random.Value.GetBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long NextLongFast()
        {
            var buffer = new byte[sizeof(long)];
            _prng.Value.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
