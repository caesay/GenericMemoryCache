using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    using System.Runtime.Caching.Generic;
    using System.Runtime.Caching.Generic.Collections;

    class Program
    {
        static void Main(string[] args)
        {
            // (See also the NUnit tests)
            var tests = new List<IPerformanceTest>();
            tests.Add(new CachingPerformanceTest(10));
            foreach (var test in tests)
            {
                Console.Clear();
                Console.WriteLine(test);
                Console.WriteLine();
                Console.WriteLine("Press ESC to skip, or any other key to start...");
                if (Console.ReadKey().KeyChar != 27)
                {
                    test.Run();
                    Console.WriteLine();
                    Console.WriteLine("... done.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}