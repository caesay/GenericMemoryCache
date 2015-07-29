using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public interface IPerformanceTest
    {
        void Run();

        string Title { get; }
    }
}