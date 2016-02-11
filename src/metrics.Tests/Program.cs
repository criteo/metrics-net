#if COREFX
using NUnitLite;
using System;
using System.Reflection;

namespace metrics.Tests
{
    public class Program
    {
        public int Main(string[] args)
        {
            return new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
        }
    }
}
#endif