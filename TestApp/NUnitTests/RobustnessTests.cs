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
    public class RobustnessTests
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
        [ExpectedException(typeof(ArgumentException))]
        public void Test001_DoubleEndedUniqueCollectionDoesntAcceptKeyDuplicates()
        {
            var delist = new DoubleEndedUniqueCollection<int>();
            delist.Add(123);
            Console.WriteLine("Added " + 123);
            delist.Add(789);
            Console.WriteLine("Added " + 789);
            delist.Add(456);
            Console.WriteLine("Added " + 456);

            var error = null as Exception;
            try
            {
                Console.WriteLine("Trying to add " + 789 + " again");
                delist.Add(789);
                Console.WriteLine("Added " + 789);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't add " + 789 + " again... removing it...");
                delist.Remove(789);
                error = ex;
            }
            Assert.AreEqual(delist.Contains(789), false);

            Console.WriteLine("Re-adding " + 789);
            delist.Add(789);
            Console.WriteLine("Added " + 789);
            Assert.AreEqual(delist.Contains(789), true);

            if (error != null)
            {
                Console.WriteLine("Re-throwing the exception we caught...");
                throw error;
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test002_CannotSetThePolicyOfANonEmptyCache()
        {
            var nonEmptyCache = new MemoryCache<int, int>();
            nonEmptyCache.Policy = nonEmptyCache.CreatePolicy(typeof(NoEvictionPolicy<,>));

            nonEmptyCache.Add(1, 1);
            nonEmptyCache.Policy = null;
        }

        [Test]
        public void Test003_CacheAddIgnoresKeyDuplicatesAndReturnsFalse()
        {
            var nonEmptyCache = new MemoryCache<int, int>();

            nonEmptyCache.Add(3, 3);
            nonEmptyCache.Add(1, 1);
            nonEmptyCache.Add(5, 5);
            nonEmptyCache.Add(4, 4);

            Assert.AreEqual(nonEmptyCache.Contains(2), false);
            Assert.AreEqual(nonEmptyCache.Contains(5), true);
            Assert.AreEqual(nonEmptyCache.Contains(4), true);

            Assert.AreEqual(nonEmptyCache.Remove(4), true);

            Assert.AreEqual(nonEmptyCache.Add(5, 123), false);
            Assert.AreEqual(nonEmptyCache.Add(4, 6), true);
            Assert.AreEqual(nonEmptyCache.Get(5), 5);
            Assert.AreEqual(nonEmptyCache.Get(4), 6);
        }

        [Test]
        public void Test004_CachePutIgnoresKeyDuplicatesAndAllowsOverwrite()
        {
            var nonEmptyCache = new MemoryCache<int, int>();

            nonEmptyCache.Add(3, 3);
            nonEmptyCache.Add(1, 1);
            nonEmptyCache.Add(5, 5);
            nonEmptyCache.Add(4, 4);

            Assert.AreEqual(nonEmptyCache.Contains(2), false);
            Assert.AreEqual(nonEmptyCache.Contains(5), true);
            Assert.AreEqual(nonEmptyCache.Contains(4), true);

            Assert.AreEqual(nonEmptyCache.Remove(4), true);

            nonEmptyCache.Put(5, 123);
            Assert.AreEqual(nonEmptyCache[5], 123);
            Assert.AreEqual(nonEmptyCache.Add(4, 6), true);
            Assert.AreEqual(nonEmptyCache[4], 6);
            nonEmptyCache.Put(4, 7);
            Assert.AreEqual(nonEmptyCache[4], 7);
        }
    }
}