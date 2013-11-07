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
        private static readonly System.Random _prng;

        public static bool UseCrypto { get; set; }

        static Random()
        {
            _prng = new System.Random();
            UseCrypto = true;
        }

        public static double NextDouble()
        {
            var l = NextLong();
            if(l == Int64.MinValue)
            {
                l = 0;
            }
            return (l + .0) / Int64.MaxValue;
        }

        public static long NextLong()
        {
            return (UseCrypto ? NextLongCrypto() : NextLongFast());
        }

        public static long NextLongCrypto()
        {
            var buffer = new byte[sizeof(long)];
            _random.Value.GetBytes(buffer);
            var value = BitConverter.ToInt64(buffer, 0);
            return value;
        }

        public static long NextLongFast()
        {
            var buffer = new byte[sizeof(long)];
            _prng.NextBytes(buffer);
            var value = BitConverter.ToInt64(buffer, 0);
            return value;
        }
    }
}