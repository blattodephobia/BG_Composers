using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public partial class LazyOrderedEnumerable<T>
    {
        private struct IterationFrame
        {
            public int StartIndex { get; private set; }

            public int EndIndex { get; private set; }

            public IterationFrame(int startIndex, int endIndex) :
                this()
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }

            public IterationFrame Left(int pivotIndex)
            {
                return new IterationFrame(StartIndex, pivotIndex - 1);
            }

            public IterationFrame Right(int pivotIndex)
            {
                return new IterationFrame(pivotIndex + 1, EndIndex);
            }
        }
    }
}
