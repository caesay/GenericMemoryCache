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
    public class StressTests
    {
        //[Test]
        //public void SomeTest()
        //{
        //}

        [SetUp]
        public void Init()
        {
        }

        public const int DOUBLEENDEDUNIQUECOLLECTION_TEST_NUMBER_OF_ITEMS = 100 * 1000;

        [Test]
        public void Test001_DoubleEndedUniqueCollectionIsMuchFasterOnRandomItemAccess()
        {
            var random = new Random((int)DateTime.Now.Millisecond);
            var accessed = new bool[DOUBLEENDEDUNIQUECOLLECTION_TEST_NUMBER_OF_ITEMS];
            var access = new int[DOUBLEENDEDUNIQUECOLLECTION_TEST_NUMBER_OF_ITEMS];
            for (var a = 0; a < access.Length; a++)
            {
                int at;
                while (accessed[at = random.Next(accessed.Length)]) ;
                accessed[at] = true;
                access[a] = at;
            }

            ICollection<int> lklist = new LinkedList<int>();
            for (var i = 0; i < accessed.Length; i++)
            {
                lklist.Add(i);
            }

            ICollection<int> delist = new DoubleEndedUniqueCollection<int>();
            for (var i = 0; i < accessed.Length; i++)
            {
                delist.Add(i);
            }

            var list = lklist;
            var time = Helpers.Measuring.Time.Start();
            for (var i = 0; i < access.Length; i++)
            {
                var value = access[i];
                list.Remove(value);
                list.Add(value);
            }
            var elapsed = time.ElapsedMilliseconds;
            Console.WriteLine();
            Console.WriteLine("LinkedList: {0} ms", elapsed.ToString("0,0"));

            list = delist;
            time = Helpers.Measuring.Time.Start();
            for (var i = 0; i < access.Length; i++)
            {
                var value = access[i];
                list.Remove(value);
                list.Add(value);
            }
            elapsed = time.ElapsedMilliseconds;
            Console.WriteLine();
            Console.WriteLine("Double-ended unique collection: {0} ms", elapsed.ToString("0,0"));

            var lklistArray = lklist.ToArray();
            var delistArray = delist.ToArray();
            Assert.AreEqual(lklistArray, delistArray);
        }
    }
}