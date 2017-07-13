using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public partial class LazyOrderedEnumerable<T, TKey>
    {
        [DebuggerDisplay("{" + nameof(StartIndex) + "}:{" + nameof(EndIndex) + "}")]
        private struct IterationFrame
        {
            public int StartIndex { get; private set; }

            public int EndIndex { get; private set; }

            public int ElementsCount => EndIndex - StartIndex + 1;

            public IterationFrame(int startIndex, int endIndex) :
                this()
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }

            public IterationFrame Left(int pivotIndex)
            {
                return new IterationFrame(StartIndex, Math.Max(StartIndex, pivotIndex - 1));
            }

            public IterationFrame Right(int pivotIndex)
            {
                return new IterationFrame(Math.Min(pivotIndex + 1, EndIndex), EndIndex);
            }
        }
    }
}
