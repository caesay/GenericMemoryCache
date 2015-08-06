using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Helpers.Fluent;
using Helpers.Measuring;

using System.Runtime.Caching.Generic;

namespace TestApp
{
    public class CachingPerformanceTest : BasePerformanceTest
    {
        // Constant working set size (total number of datums)
        const int TOTAL_DATUM_COUNT = 1000000;

        public CachingPerformanceTest(int loadFactor)
            : base("Caching Performance Test")
        {
            RandomAccessCount = loadFactor * TOTAL_DATUM_COUNT;
        }

        // Number of random accesses we will perform on each caching facility (a multiple of TOTAL_DATUM_COUNT)
        protected int RandomAccessCount { get; private set; }

        public override void Run()
        {
            Console.WriteLine();
            Console.WriteLine("Preparing raw data...");
            var random = new Random(DateTime.Now.Millisecond);
            var access = new int[RandomAccessCount];
            var data = new Datum[TOTAL_DATUM_COUNT];
            for (var d = 0; d < data.Length; d++)
            {
                data[d] =
                    new Datum
                    {
                        Id = d + 1,
                        SomePayload = Enumerable.Range(0, 26).Aggregate(new StringBuilder(), (sb, i) => sb.Append('A' + random.Next(26))).ToString()
                    };
            }
            // To be fair, we will access each caching facility with the exact same sequence (of length RandomAccessCount)
            // of datum indices (picked randomly in the range 0...TOTAL_DATUM_COUNT - 1, inclusive)
            // (Depending on the ratio RANDOM_ACCESS_COUNT / TOTAL_DATUM_COUNT,
            // some indices in the range 0...TOTAL_DATUM_COUNT - 1 may or may not ever be returned by the below random.Next(...))
            for (var a = 0; a < access.Length; a++)
            {
                access[a] = random.Next(TOTAL_DATUM_COUNT);
            }
            Console.WriteLine();
            Console.WriteLine("... raw data prepared.");

            Console.WriteLine();
            Console.WriteLine("Preparing caches...");
            var msMemCache = new System.Runtime.Caching.MemoryCache("System.Runtime.Caching.MemoryCache");
            var msPolicy = new System.Runtime.Caching.CacheItemPolicy();
            msPolicy.Priority = System.Runtime.Caching.CacheItemPriority.Default;

            IMemoryCache ourMemCache = new MemoryCache<int, Datum>(TOTAL_DATUM_COUNT / 5);
            ourMemCache.SetPolicy(typeof(MruEvictionPolicy<,>));
            Console.WriteLine();
            Console.WriteLine("... caches prepared.");

            var accessPercent = RandomAccessCount / 100;

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("About to stress Microsoft's {0}...", typeof(System.Runtime.Caching.MemoryCache).FullName);
            Console.WriteLine();
            Console.WriteLine("Total datum count : {0}", TOTAL_DATUM_COUNT.ToString("0,0"));
            Console.WriteLine();
            Console.WriteLine("Number of random accesses to perform : {0}", RandomAccessCount.ToString("0,0"));
            Console.WriteLine();
            Console.WriteLine("Press any key (and hang on)...");
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine("Time... {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.WriteLine();

            var time1 = Time.Start();
            var period1 = 0d;
            for (var a = 0; a < access.Length; a++)
            {
                var index = access[a];
                var datum = data[index];
                var item = (Datum)msMemCache.AddOrGetExisting(datum.Id.ToString(), datum, msPolicy) ?? datum;
                if (item.Id != datum.Id || item.SomePayload != datum.SomePayload)
                {
                    throw new Exception("Ouch. Unexpected item.");
                }
                if (a % accessPercent == 0)
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Time... {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.WriteLine();
            Console.WriteLine("Elapsed: {0} ms", (period1 = time1.ElapsedMilliseconds).ToString("0,0"));
            Console.WriteLine("Latency: {0} ms (avg.)", period1 / RandomAccessCount);

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("About to stress our System.Runtime.Caching.Generic.MemoryCache...");
            Console.WriteLine();
            Console.WriteLine("Total datum count : {0}", TOTAL_DATUM_COUNT.ToString("0,0"));
            Console.WriteLine();
            Console.WriteLine("Number of random accesses to perform : {0}", RandomAccessCount.ToString("0,0"));
            Console.WriteLine();
            Console.WriteLine("Press any key (and hang on)...");
            Console.WriteLine();
            Console.WriteLine("(# of cache evictions shown in parentheses)...");
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine("Time... {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.WriteLine();

            var evictedCount = 0;
            ourMemCache.Policy.OnEvict =
                delegate(IManagedCache<int, Datum> source, int key, Datum value, EvictionReason reason)
                {
                    evictedCount++;
                };
            var time2 = Time.Start();
            var period2 = 0d;
            var misses = 0;
            for (var a = 0; a < access.Length; a++)
            {
                var index = access[a];
                var datum = data[index];
                var item = ourMemCache.GetOrAdd(datum.Id, (Func<object, Datum>)delegate(object context){ misses++; return datum; });
                if (item.Id != datum.Id || item.SomePayload != datum.SomePayload)
                {
                    throw new Exception("Ouch. Unexpected item.");
                }
                if (a % accessPercent == 0)
                {
                    Console.Write("({0})", evictedCount);
                    evictedCount = 0;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Time... {0}", DateTime.Now.ToString("HH:mm:ss.fff"));
            Console.WriteLine();
            Console.WriteLine("Elapsed: {0} ms", (period2 = time2.ElapsedMilliseconds).ToString("0,0"));
            Console.WriteLine("Latency: {0} ms (avg.)", period2 / RandomAccessCount);
            Console.WriteLine("Initial cache capacity: {0}", ourMemCache.Capacity.ToString("0,0"));
            Console.WriteLine("Final cache size: {0}", ourMemCache.Count.ToString("0,0"));
            Console.WriteLine("No. cache misses: {0}", misses.ToString("0,0"));
            Console.WriteLine("Cache fill ratio: {0}%", (100 * ourMemCache.Count / ourMemCache.Capacity).ToString(".00"));
        }

        public class Datum
        {
            public int Id { get; set; }
            public string SomePayload { get; set; }
        }
    }
}