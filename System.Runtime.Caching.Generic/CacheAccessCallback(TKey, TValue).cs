using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public delegate void CacheAccessCallback<TKey, TValue>(IManagedCache<TKey, TValue> source, TKey key, TValue value);
}