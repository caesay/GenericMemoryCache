using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public abstract class AbstractEvictingCache<TKey, TValue> : AbstractCache, IEvictingCache<TKey, TValue>
    {
        private ICachePolicy<TKey, TValue> policy;

        protected AbstractEvictingCache(int capacity)
            : this(null, capacity)
        {
        }

        protected AbstractEvictingCache(IManagedCache<TKey, TValue> parent, int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "must be strictly greater than zero");
            }
            Parent = parent;
        }

        protected abstract void Prepare(ICachePolicy<TKey, TValue> policy);

        protected abstract IDictionary<TKey, TValue> GetSnapshot();

        #region IEvictingCache<TKey, TValue> implementation
        public ICachePolicy<TKey, TValue> CreatePolicy(Type genericPolicyType, params object[] args)
        {
            if (genericPolicyType == null)
            {
                throw new ArgumentNullException("genericPolicyType", "cannot be null");
            }
            return (ICachePolicy<TKey, TValue>)Activator.CreateInstance(genericPolicyType.MakeGenericType(typeof(TKey), typeof(TValue)), args);
        }

        public void SetPolicy(Type genericPolicyType, params object[] args)
        {
            Policy = CreatePolicy(genericPolicyType, args);
        }

        public ICachePolicy<TKey, TValue> Policy
        {
            get
            {
                return policy;
            }
            set
            {
                if (Count > 0)
                {
                    throw new InvalidOperationException("cannot set the policy of a non-empty cache");
                }
                Prepare(policy = value);
            }
        }
        #endregion

        #region IManagedCache<TKey, TValue> implementation
        public abstract bool Contains(TKey key);

        public abstract bool TryGet(TKey key, out TValue cached);

        public abstract TValue Get(TKey key);

        public abstract TValue GetOrAdd(TKey key, TValue value);

        public abstract TValue GetOrAdd<TContext>(TKey key, Func<TContext, TValue> updater, TContext context);

        public abstract bool Add(TKey key, TValue value);

        public abstract void Put(TKey key, TValue value);

        public abstract bool Remove(TKey key);

        public IManagedCache<TKey, TValue> Parent { get; private set; }
        #endregion

        #region IAbstractCache<TKey, TValue> implementation
        public TValue this[TKey key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Put(key, value);
            }
        }

        public abstract int Capacity { get; }

        public abstract int Count { get; }
        #endregion

        #region IEnumerable<TKey> implementation
        public IEnumerator<TKey> GetEnumerator()
        {
            return GetSnapshot().Keys.GetEnumerator();
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