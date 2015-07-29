using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the interface to a N-way set-associative memory cache.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface IMemoryCache<TKey, TValue> : IEvictingCache<TKey, TValue>
    {
        /// <summary>
        /// Gets the indexer used by the cache to dispatch a given key onto one of its N ways / sub-caches (surjection: TKey -> [0...N-1]).
        /// </summary>
        Func<TKey, int> Indexer { get; }
    }
}