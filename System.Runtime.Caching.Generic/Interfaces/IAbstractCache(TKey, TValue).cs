using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the minimal interface of a strongly-typed cache.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface IAbstractCache<TKey, TValue> : IEnumerable<TKey>
    {
        /// <summary>
        /// Gets or sets a value in the cache, by key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        TValue this[TKey key] { get; set; }

        /// <summary>
        /// Gets the cache's capacity.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Gets the cache's size (i.e., the number of cached key/value pairs).
        /// </summary>
        int Count { get; }
    }
}