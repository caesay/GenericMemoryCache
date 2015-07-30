System.Runtime.Caching.Generic
==============================

* <a href="#Overview">Overview</a>
* <a href="#Sample">Sample use</a>

<a name="Overview"></a>

Overview
--------

This is a lightweight, strongly-typed, generic, extensible, and thread-safe, N-way set-associative, in-process (memory) cache, coming with 4 built-in eviction / replacement policies (LRU, MRU, LFU, none).

The TestApp console application accompanying the library comes with a few (NUnit) tests wrt. correctness, robustness, and extensibility, along with a basic comparative performance / stress test vs. Microsoft's System.Runtime.Caching introduced in .NET 4.0.

**Please read and accept** the terms of the [LICENSE](https://raw.githubusercontent.com/ysharplanguage/GenericMemoryCache/master/LICENSE.md), or else, do not use this library *as-is*.

<a name="Sample"></a>

Sample use
----------

        [Test]
        // LFU: Least Frequently Used
        public void Test_BasicLFUCacheSemantic()
        {
            var lfuCache = new MemoryCache<int, string>();
            lfuCache.Policy = lfuCache.CreatePolicy(typeof(LfuEvictionPolicy<,>));
            
            // Note the default number of ways is 1, and the default capacity is 16...
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
