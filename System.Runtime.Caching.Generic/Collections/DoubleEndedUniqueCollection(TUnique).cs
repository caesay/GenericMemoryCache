using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic.Collections
{
    /*
     * A (doubly) linked list that isn't supposed to ever contain duplicates can be optimized
     * greatly for random item accesses (to n-th item), to make the latter O(1) instead of O(n);
     * and that's exactly what we need to implement efficiently the collection of evictable keys
     * for the LRU/MRU cache evictions and replacements.
     */
    public class DoubleEndedUniqueCollection<TUnique> : IDoubleEndedUniqueCollection<TUnique>
    {
        private LinkedList<TUnique> items;
        private IDictionary<TUnique, LinkedListNode<TUnique>> hooks;

        public DoubleEndedUniqueCollection()
            : this(0)
        {
        }

        public DoubleEndedUniqueCollection(int capacity)
        {
            items = new LinkedList<TUnique>();
            hooks = new Dictionary<TUnique, LinkedListNode<TUnique>>(capacity);
        }

        #region IDoubleEndedUniqueCollection<TUnique> implementation
        public TUnique First
        {
            get
            {
                if (items.First == null)
                {
                    throw new InvalidOperationException("list is empty");
                }
                return items.First.Value;
            }
        }

        public TUnique Last
        {
            get
            {
                if (items.Last == null)
                {
                    throw new InvalidOperationException("list is empty");
                }
                return items.Last.Value;
            }
        }
        #endregion

        #region ICollection<TUnique> implementation
        public void Add(TUnique value)
        {
            var item = new LinkedListNode<TUnique>(value);
            hooks.Add(value, item);
            items.AddLast(item);
        }

        public void Clear()
        {
            items.Clear();
            hooks.Clear();
        }

        public bool Contains(TUnique value)
        {
            return hooks.ContainsKey(value);
        }

        public void CopyTo(TUnique[] array, int index)
        {
            var values = items.ToArray();
            values.CopyTo(array, index);
        }

        public bool Remove(TUnique value)
        {
            LinkedListNode<TUnique> item;
            bool found;
            if (found = hooks.TryGetValue(value, out item))
            {
                items.Remove(item);
                hooks.Remove(value);
            }
            return found;
        }

        public int Count
        {
            get
            {
                return items.Count;
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

        #region IEnumerable<TUnique> implementation
        public IEnumerator<TUnique> GetEnumerator()
        {
            return ((IEnumerable<TUnique>)items).GetEnumerator();
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