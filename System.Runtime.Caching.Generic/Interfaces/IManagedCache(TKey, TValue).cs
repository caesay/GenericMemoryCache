using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    /// <summary>
    /// Represents the interface of a managed cache, which may or may not have a parent cache.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the cache</typeparam>
    /// <typeparam name="TValue">The type of values in the cache</typeparam>
    public interface IManagedCache<TKey, TValue> : IAbstractCache<TKey, TValue>
    {
        /// <summary>
        /// Determines whether the cache contains a value for the given key. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(TKey key);

        /// <summary>
        /// Attempts to get the value for the given key.
        /// </summary>
        /// <param name="key">The value's key</param>
        /// <param name="value">The existing cached value</param>
        /// <returns>True, if the key/value pair is found, false otherwise</returns>
        bool TryGet(TKey key, out TValue value);

        /// <summary>
        /// Gets the value for the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue Get(TKey key);

        /// <summary>
        /// Gets or adds the given value for the given key. For an evicting cache, this may or may not cause an eviction (eviction reason: Policy).
        /// </summary>
        /// <param name="key">The value's key</param>
        /// <param name="value">The value to add, if the key isn't found</param>
        /// <returns>The existing cached value, if any, or value, the second parameter</returns>
        TValue GetOrAdd(TKey key, TValue value);

        /// <summary>
        /// Gets or adds the given value for the given key. For an evicting cache, this may or may not cause an eviction (eviction reason: Policy).
        /// </summary>
        /// <param name="key">The value's key</param>
        /// <param name="updater">The updater that obtains the value to add, if the key isn't found</param>
        /// <param name="context">The update context to use when calling the updater</param>
        /// <returns>The existing cached value, if any, or the value obtained from the updater, the second parameter</returns>
        TValue GetOrAdd<TContext>(TKey key, Func<TContext, TValue> updater, TContext context = default(TContext));

        /// <summary>
        /// Attempts to add the given value for the given key. For an evicting cache, this may or may not cause an eviction (eviction reason: Policy).
        /// </summary>
        /// <param name="key">The value's key</param>
        /// <param name="value">The value to add</param>
        /// <returns>True, if the operation succeeded, false otherwise</returns>
        bool Add(TKey key, TValue value);

        /// <summary>
        /// Puts the given value, for the given key. For an evicting cache, this may or may not cause an eviction (eviction reason: Policy).
        /// </summary>
        /// <param name="key">The value's key</param>
        /// <param name="value">The value to put</param>
        void Put(TKey key, TValue value);

        /// <summary>
        /// Attempts to remove the value for the given key. For an evicting cache, this causes an eviction (eviction reason: Removal).
        /// </summary>
        /// <param name="key">The value's key</param>
        /// <returns>True, if the operation succeeded, false otherwise</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Gets the parent cache, if any.
        /// </summary>
        IManagedCache<TKey, TValue> Parent { get; }
    }
}