using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents a cache policy specifying the eviction strategy, the eviction priority, and the client callbacks that can be used by an evicting cache.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface ICachePolicy<TKey, TValue>
    {
        /// <summary>
        /// Returns the type that implements the eviction strategy to use by an evicting cache which uses this cache policy.
        /// </summary>
        /// <returns>The type of eviction strategy to use</returns>
        Type GetCacheEvictionType();

        /// <summary>
        /// Gets or sets the callback delegate that notifies the client when a value is read from an evicting cache which uses this cache policy.
        /// </summary>
        CacheAccessCallback<TKey, TValue> OnGet { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that notifies the client when a value is added to an evicting cache which uses this cache policy.
        /// </summary>
        CacheAccessCallback<TKey, TValue> OnAdd { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that notifies the client when a value is updated in an evicting cache which uses this cache policy.
        /// </summary>
        CacheAccessCallback<TKey, TValue> OnPut { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that notifies the client when a value is evicted from an evicting cache which uses this cache policy.
        /// </summary>
        CacheEvictionCallback<TKey, TValue> OnEvict { get; set; }

        /// <summary>
        /// Gets the policy's eviction priority.
        /// </summary>
        EvictionPriority EvictionPriority { get; }
    }
}