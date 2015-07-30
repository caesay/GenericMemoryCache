using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic.Collections
{
    public class LfuEvictableCollection<TKey> : IUseTrackingCollection<TKey>
    {
        internal sealed class KeyUse
        {
            internal const int None = -1;

            private long value;

            private int msb = None;

            internal int Increment()
            {
                var mask = 1L << (msb + 1);
                if ((++value & mask) == mask)
                {
                    msb++;
                }
                return msb;
            }

            internal long Count { get { return value; } }

            // Reflects the logarithm base 2 of the use count
            internal int Msb { get { return msb; } }
        }

        private IDictionary<TKey, KeyUse> uses;

        // We dispatch used keys in use counts partitions, indexed by
        // the logarithm base 2 of the keys' use counts; thus,
        // the single set of keys which have been accessed only once will be in keys[0];
        // the (up to) 2 sets of keys which have been accessed 2 to 3 times, in keys[1];
        // the (up to) 4 sets of keys which have been accessed 4 to 7 times, in keys[2];
        // the (up to) 8 sets of keys which have been accessed 8 to 15 times, in keys[3];
        // the (up to) 16 sets of keys which have been accessed 16 to 31 times, in keys[4];
        // etc, etc.
        private IDictionary<int, HashSet<TKey>> keys;

        public LfuEvictableCollection()
            : this(0)
        {
        }

        public LfuEvictableCollection(int capacity)
        {
            uses = new Dictionary<TKey, KeyUse>(capacity);
            keys = new Dictionary<int, HashSet<TKey>>(Msb(long.MaxValue) + 1);
            Initialize();
        }

        // Get the most significant bit (index) of n, assumed positive
        // (for long.MaxValue, signed 64-bit integer maximum, this is 62)
        // or -1, if n is zero
        private static int Msb(long n)
        {
            if (n > 0)
            {
                var max = long.MaxValue;
                var msb = 0;
                while (n <= max) max /= 2;
                max += 1;
                while (0 < (max /= 2)) msb++;
                return msb;
            }
            return KeyUse.None;
        }

        // Get the logarithm base 2 of the least often used keys' use count,
        // or -1 if no keys have ever been used
        private int Min()
        {
            var msb = 0;
            while (msb < keys.Count && keys[msb].Count < 1)
            {
                msb++;
            }
            return msb < keys.Count ? msb : KeyUse.None;
        }

        protected virtual void Initialize()
        {
            TotalUseCount = 0;
            keys.Clear();
            uses.Clear();
        }

        #region IUseTrackingCollection<TKey> implementation
        public void Use(TKey key)
        {
            var use = uses[key];
            var last = use.Msb;
            var next = use.Increment();
            TotalUseCount++;
            if (last < next)
            {
                if (last > KeyUse.None)
                {
                    keys[last].Remove(key);
                }
                if (!keys.ContainsKey(next))
                {
                    keys.Add(next, new HashSet<TKey>());
                }
                keys[next].Add(key);
            }
        }

        public IEnumerable<TKey> GetLeastFrequentlyUsed(int maxCount)
        {
            if (maxCount <= 0)
            {
                throw new ArgumentOutOfRangeException("maxCount", "must be strictly greater than zero");
            }
            var msb = Min();
            if (msb > KeyUse.None)
            {
                while (maxCount > 0 && msb < this.keys.Count)
                {
                    var keys = this.keys[msb];
                    foreach (var key in keys)
                    {
                        maxCount--;
                        yield return key;
                    }
                    msb++;
                }
            }
            if (maxCount > 0)
            {
                foreach (var key in this.uses.Keys)
                {
                    if (maxCount > 0)
                    {
                        maxCount--;
                        yield return key;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public long TotalUseCount { get; private set; }
        #endregion

        #region ICollection<TKey> implementation
        public void Add(TKey key)
        {
            var use = new KeyUse();
            uses.Add(key, use);
        }

        public void Clear()
        {
            Initialize();
        }

        public bool Contains(TKey key)
        {
            return uses.ContainsKey(key);
        }

        public void CopyTo(TKey[] array, int index)
        {
            var keys = uses.Keys.ToArray();
            keys.CopyTo(array, index);
        }

        public bool Remove(TKey key)
        {
            KeyUse use;
            if (uses.TryGetValue(key, out use))
            {
                var msb = use.Msb;
                TotalUseCount -= use.Count;
                if (keys.ContainsKey(msb))
                {
                    keys[msb].Remove(key);
                }
                uses.Remove(key);
                return true;
            }
            return false;
        }

        public int Count
        {
            get
            {
                return uses.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region IEnumerable<TKey> implementation
        public IEnumerator<TKey> GetEnumerator()
        {
            return ((IEnumerable<TKey>)uses.Keys).GetEnumerator();
        }
        #endregion

        #region IEnumerable implementation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}