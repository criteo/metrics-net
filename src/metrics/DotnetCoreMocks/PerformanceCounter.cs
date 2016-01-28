using System;

namespace metrics.DotnetCoreMocks
{
    internal class PerformanceCounter
    {
        internal string CounterName { get; set; }
        internal double NextValue()
        {
            throw new NotSupportedException();
        }

        internal static void CloseSharedResources()
        {

        }

    }
}
