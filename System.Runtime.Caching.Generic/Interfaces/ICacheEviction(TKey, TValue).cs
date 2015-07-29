using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents an eviction strategy (and associated unitary eviction operation), that can be used by an evicting cache.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface ICacheEviction<TKey, TValue> : ICacheState<TKey, TValue>
    {
        /// <summary>
        /// Attempts to evict a single value, by key, from the evicting cache which uses this eviction strategy.
        /// </summary>
        /// <param name="reason">The eviction reason</param>
        /// <param name="key">The key</param>
        /// <returns>True if the operation succeeded, false otherwise</returns>
        bool Evict(EvictionReason reason, TKey key);

        /// <summary>
        /// Attempts to evict as many values as this eviction strategy is supposed to, from the evicting cache which uses it.
        /// </summary>
        /// <returns>True if the operation succeeded, false otherwise</returns>
        bool Evict();
    }
}