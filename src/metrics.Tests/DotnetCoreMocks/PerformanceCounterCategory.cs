using metrics.DotnetCoreMocks;

namespace metrics.Tests.DotnetCoreMocks
{
    internal class PerformanceCounterCategory
    {
        internal PerformanceCounterCategoryType CategoryType { get; set; }
        internal string CategoryName { get; set; }

        internal PerformanceCounterCategory(string category)
        {
            CategoryName = category;
        }

        internal static PerformanceCounterCategory[] GetCategories()
        {
            return new PerformanceCounterCategory[0];
        }

        internal PerformanceCounter[] GetCounters(string instance = "")
        {
            return new PerformanceCounter[0];
        }

        internal string[] GetInstanceNames()
        {
            return new string[0];
        }
    }
    internal enum PerformanceCounterCategoryType
    {
        SingleInstance
    }
}
