using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Caching.Generic;
using System.Runtime.Caching.Generic.Collections;

namespace TestApp
{
    using NUnit.Framework;

    [TestFixture]
    public class ExtensibilityTests
    {
        //[Test]
        //public void SomeTest()
        //{
        //}

        [SetUp]
        public void Init()
        {
        }

        [Test]
        public void Test001_AdhocHitFrequencyRecordingPolicy()
        {
            var data =
                Enumerable.Range(0, AbstractCache.DefaultCapacity + 1).
                Aggregate(new StringBuilder(), (sb, i) => sb.Append(i > 0 ? String.Format(",String {0}", i) : String.Empty)).
                ToString().
                Split(',');
            var hits = new int[data.Length];
            var expected = new int[data.Length];
            for (var i = 1; i < data.Length; i++)
            {
                expected[i] = 4;
            }
            expected[3] = 3;
            expected[9] = 2;

            var hfrCache = new MemoryCache<int, string>();
            hfrCache.SetPolicy(typeof(NoEvictionPolicy<,>));
            hfrCache.Policy.OnGet =
                delegate(IManagedCache<int, string> source, int key, string value)
                {
                    hits[key]++;
                };

            Assert.AreEqual(hfrCache.Capacity, AbstractCache.DefaultCapacity);

            // Cache all the (16) non-empty strings
            for (var i = 1; i < data.Length; i++)
            {
                Assert.AreEqual(hfrCache.Add(i, data[i]), true);
            }

            // Use all the (16) non-empty strings thrice...
            for (var use = 1; use <= 3; use++)
            {
                for (var i = 1; i < data.Length; i++)
                {
                    // ... except for String 3, used only twice...
                    if (i == 3)
                    {
                        if (use <= 2)
                        {
                            var s = hfrCache[i];
                        }
                    }
                    // ... and for String 9, used only once
                    else if (i == 9)
                    {
                        if (use <= 1)
                        {
                            var s = hfrCache[i];
                        }
                    }
                    else
                    {
                        var s = hfrCache[i];
                    }
                }
            }

            foreach (var key in hfrCache)
            {
                Console.WriteLine("{0}'s # hits so far: {1}...", key, hits[key]);
            }
            Assert.AreEqual(hits, expected);
        }

        [Test]
        // LFU: Least Frequently Used
        public void Test002_BasicLFUCacheSemantic()
        {
            var lfuCache = new MemoryCache<int, string>();
            lfuCache.SetPolicy(typeof(LfuEvictionPolicy<,>));
            Assert.AreEqual(lfuCache.Capacity, AbstractCache.DefaultCapacity);

            var data =
                Enumerable.Range(0, AbstractCache.DefaultCapacity + 1).
                Aggregate(new StringBuilder(), (sb, i) => sb.Append(i > 0 ? String.Format(",String {0}", i) : String.Empty)).
                ToString().
                Split(',');

            // Cache all the (16) non-empty strings
            for (var i = 1; i < data.Length; i++)
            {
                lfuCache.Add(i, data[i]);
            }

            // Use all the (16) non-empty strings four times...
            for (var use = 1; use <= 4; use++)
            {
                for (var i = 1; i < data.Length; i++)
                {
                    // ... except for String 3, used only twice...
                    if (i == 3)
                    {
                        if (use <= 2)
                        {
                            var s = lfuCache[i];
                        }
                    }
                    // ... and for String 9, used only once
                    else if (i == 9)
                    {
                        if (use <= 1)
                        {
                            var s = lfuCache[i];
                        }
                    }
                    else
                    {
                        var s = lfuCache[i];
                    }
                }
            }

            lfuCache.Add(17, "String 17");
            Assert.AreEqual(lfuCache.Contains(9), false);
            Assert.AreEqual(lfuCache.Contains(17), true);

            var used4times = lfuCache.Get(17);
            used4times = lfuCache.Get(17);
            used4times = lfuCache.Get(17);
            used4times = lfuCache.Get(17);

            lfuCache.Put(18, "String 18");
            Assert.AreEqual(lfuCache.Contains(3), false);
            Assert.AreEqual(lfuCache.Contains(17), true);
            Assert.AreEqual(lfuCache.Contains(18), true);
        }
    }
}