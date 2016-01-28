
namespace metrics.DotnetCoreMocks
{
    internal class PerformanceCounter
    {
        internal string CounterName { get; set; }
        internal double NextValue()
        {
            return 1.0d;
        }

        internal static void CloseSharedResources()
        {

        }

    }
}
