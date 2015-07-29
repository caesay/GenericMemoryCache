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
    public class CorrectnessTests
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
        public void Test001_DoubleEndedUniqueCollectionBehavesLikeLinkedList()
        {
            var lklist = new LinkedList<int>();
            lklist.AddLast(456);
            lklist.AddLast(123);
            lklist.AddLast(789);
            lklist.AddLast(567);
            lklist.Remove(123);

            var delist = new DoubleEndedUniqueCollection<int>();
            delist.Add(456);
            delist.Add(123);
            delist.Add(789);
            delist.Add(567);
            delist.Remove(123);

            var expectedArray = new int[] { 456, 789, 567 };
            var lklistArray = lklist.ToArray();
            var delistArray = delist.ToArray();

            Assert.AreEqual(expectedArray, lklistArray);
            Assert.AreEqual(expectedArray, delistArray);
        }

        [Test]
        public void Test002_NoEvictionCacheJustKeepsGrowing()
        {
            var noEvictionCache = new MemoryCache<int, int>();
            Assert.AreEqual(noEvictionCache.Capacity, AbstractCache.DefaultCapacity);

            noEvictionCache.Add(19, 19);
            noEvictionCache.Add(1, 1);
            noEvictionCache.Add(6, 6);
            noEvictionCache.Add(2, 2);
            noEvictionCache.Add(16, 16);
            noEvictionCache.Add(5, 5);
            noEvictionCache.Add(18, 18);
            noEvictionCache.Add(7, 7);
            noEvictionCache.Add(15, 15);
            noEvictionCache.Add(4, 4);
            noEvictionCache.Add(14, 14);
            noEvictionCache.Add(9, 9);
            noEvictionCache.Add(13, 13);
            noEvictionCache.Add(17, 17);
            noEvictionCache.Add(12, 12);
            noEvictionCache.Add(3, 3);
            Assert.AreEqual(noEvictionCache.Count, 16);

            noEvictionCache.Add(10, 10);
            noEvictionCache.Add(11, 11);
            noEvictionCache.Add(8, 8);
            Assert.AreEqual(noEvictionCache.Count, 19);

            var expectedArray = Enumerable.Range(1, 19).ToArray();
            var cacheArray = noEvictionCache.OrderBy(i => i).ToArray();
            Assert.AreEqual(expectedArray, cacheArray);
        }

        [Test]
        public void Test003_BasicLRUCacheSemantic()
        {
            var lruCache = new MemoryCache<int, int>();
            lruCache.Policy = lruCache.CreatePolicy(typeof(LruEvictionPolicy<,>));

            Assert.AreEqual(lruCache.Capacity, AbstractCache.DefaultCapacity);

            lruCache.Add(19, 19);
            lruCache.Add(1, 1);
            lruCache.Add(6, 6);
            lruCache.Add(2, 2);
            lruCache.Add(16, 16);
            lruCache.Add(5, 5);
            lruCache.Add(18, 18);
            lruCache.Add(7, 7);
            lruCache.Add(15, 15);
            lruCache.Add(4, 4);
            lruCache.Add(14, 14);
            lruCache.Add(9, 9);
            lruCache.Add(13, 13);
            lruCache.Add(17, 17);
            lruCache.Add(12, 12);
            lruCache.Add(3, 3);
            Assert.AreEqual(lruCache.Count, 16);

            Assert.AreEqual(lruCache.Contains(123), false);
            lruCache.Add(123, 123);
            Assert.AreEqual(lruCache.Contains(123), true);
            Assert.AreEqual(lruCache.Contains(19), false);
            Assert.AreEqual(lruCache.Count, 16);
        }

        [Test]
        public void Test004_BasicThreeWayLRUCacheSemanticDefaultIndexer()
        {
            var lruCacheDefault = new MemoryCache<int, int>(15, 3);
            lruCacheDefault.Policy = lruCacheDefault.CreatePolicy(typeof(LruEvictionPolicy<,>));

            Assert.AreEqual(lruCacheDefault.Capacity, 15);

            lruCacheDefault.Add(19, 19);   // will go in way 1
            lruCacheDefault.Add(1, 1);     // will go in way 1
            lruCacheDefault.Add(6, 6);     // will go in way 0
            lruCacheDefault.Add(4, 4);     // will go in way 1
            lruCacheDefault.Add(16, 16);   // will go in way 1
            lruCacheDefault.Add(5, 5);     // will go in way 2 (should be first to be evicted in that way)
            lruCacheDefault.Add(18, 18);   // will go in way 0
            lruCacheDefault.Add(7, 7);     // will go in way 1
            lruCacheDefault.Add(15, 15);   // will go in way 0
            lruCacheDefault.Add(2, 2);     // will go in way 2
            lruCacheDefault.Add(14, 14);   // will go in way 2
            lruCacheDefault.Add(9, 9);     // will go in way 0
            lruCacheDefault.Add(20, 20);   // will go in way 2
            lruCacheDefault.Add(17, 17);   // will go in way 2
            lruCacheDefault.Add(12, 12);   // will go in way 0
            Assert.AreEqual(lruCacheDefault.Count, 15);

            Assert.AreEqual(lruCacheDefault.Contains(125), false);
            lruCacheDefault.Put(125, 125); // will go (after causing an evict) in way 2
            Assert.AreEqual(lruCacheDefault.Contains(125), true);
            Assert.AreEqual(lruCacheDefault.Contains(5), false);
            Assert.AreEqual(lruCacheDefault.Count, 15);
        }

        [Test]
        public void Test005_BasicThreeWayLRUCacheSemanticExplicitIndexer()
        {
            var lruCache = new MemoryCache<int, int>(15, 3, k => k % 3);
            lruCache.Policy = lruCache.CreatePolicy(typeof(LruEvictionPolicy<,>));

            Assert.AreEqual(lruCache.Capacity, 15);

            lruCache.Add(19, 19);   // will go in way 1
            lruCache.Add(1, 1);     // will go in way 1
            lruCache.Add(6, 6);     // will go in way 0
            lruCache.Add(4, 4);     // will go in way 1
            lruCache.Add(16, 16);   // will go in way 1
            lruCache.Add(5, 5);     // will go in way 2 (should be first to be evicted in that way)
            lruCache.Add(18, 18);   // will go in way 0
            lruCache.Add(7, 7);     // will go in way 1
            lruCache.Add(15, 15);   // will go in way 0
            lruCache.Add(2, 2);     // will go in way 2
            lruCache.Add(14, 14);   // will go in way 2
            lruCache.Add(9, 9);     // will go in way 0
            lruCache.Add(20, 20);   // will go in way 2
            lruCache.Add(17, 17);   // will go in way 2
            lruCache.Add(12, 12);   // will go in way 0
            Assert.AreEqual(lruCache.Count, 15);

            Assert.AreEqual(lruCache.Contains(125), false);
            lruCache.Put(125, 125); // will go (after causing an evict) in way 2
            Assert.AreEqual(lruCache.Contains(125), true);
            Assert.AreEqual(lruCache.Contains(5), false);
            Assert.AreEqual(lruCache.Count, 15);
        }

        [Test]
        public void Test006_BasicMRUCacheSemantic()
        {
            var mruCache = new MemoryCache<int, int>();
            mruCache.Policy = mruCache.CreatePolicy(typeof(MruEvictionPolicy<,>));
            Assert.AreEqual(mruCache.Capacity, AbstractCache.DefaultCapacity);

            mruCache.Add(19, 19);
            mruCache.Add(1, 1);
            mruCache.Add(6, 6);
            mruCache.Add(2, 2);
            mruCache.Add(16, 16);
            mruCache.Add(5, 5);
            mruCache.Add(18, 18);
            mruCache.Add(7, 7);
            mruCache.Add(15, 15);
            mruCache.Add(4, 4);
            mruCache.Add(14, 14);
            mruCache.Add(9, 9);
            mruCache.Add(13, 13);
            mruCache.Add(17, 17);
            mruCache.Add(12, 12);
            mruCache.Add(3, 3);
            Assert.AreEqual(mruCache.Count, 16);

            Assert.AreEqual(mruCache.Contains(456), false);
            mruCache.Add(456, 456);
            Assert.AreEqual(mruCache.Contains(456), true);
            Assert.AreEqual(mruCache.Contains(3), false);
            Assert.AreEqual(mruCache.Count, 16);
        }
    }
}