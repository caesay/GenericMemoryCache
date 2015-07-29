using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the internal state, and the bookkeeping thereof, of a strongly-typed cache, which may or may not be an evicting cache
    /// (i.e., one using an eviction strategy).
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface ICacheState<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Handles the given cache access operation, along with the given (mandatory) key and (optional) value.
        /// </summary>
        /// <param name="access">The cache access operation</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="isGetOrPut">Whether the key must exist (true) or not (false) in this cache state</param>
        /// <returns>True, if the operation succeeded, false otherwise</returns>
        bool Handle(CacheAccess access, TKey key, ref TValue value, bool isGetOrPut);
    }
}