using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Measuring
{
    public sealed class Memory
    {
        private long m;
        private Memory() { Reset(); }
        private long Total() { System.Threading.Thread.MemoryBarrier(); return System.GC.GetTotalMemory(true); }
        public static Memory Mark() { return new Memory(); }
        public long Reset() { return m = Total(); }
        public long UsedBytes { get { return Total() - m; } }
    }

    public sealed class Time
    {
        private System.Diagnostics.Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
        private Time() { }
        public static Time Start() { return new Time(); }
        public long Reset() { w.Stop(); var t = (long)w.ElapsedMilliseconds; w.Restart(); return t; }
        public long ElapsedMilliseconds { get { return Reset(); } }
    }
}