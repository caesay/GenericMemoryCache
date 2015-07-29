using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Caching.Generic
{
    public abstract class AbstractCache
    {
        public const int DefaultCapacity = 16;

        public const int DefaultNumberOfWays = 1;
    }
}