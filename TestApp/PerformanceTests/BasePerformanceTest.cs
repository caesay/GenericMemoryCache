using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public abstract class BasePerformanceTest : IPerformanceTest
    {
        protected BasePerformanceTest(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }

        public abstract void Run();

        public string Title { get; private set; }
    }
}