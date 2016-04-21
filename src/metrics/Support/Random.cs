using System.Threading;

namespace metrics.Support
{
    /// <summary>
    /// Provides statistically relevant random number generation
    /// </summary>
    public class Random
    {
        private static readonly ThreadLocal<System.Random> _prng = new ThreadLocal<System.Random>(() => new System.Random());

        public static long NextLong()
        {
            long heavy = _prng.Value.Next();
            long light = _prng.Value.Next();

            return heavy << 32 | light;
        }

        public static double NextDouble()
        {
            return _prng.Value.NextDouble();
        }

    }
}