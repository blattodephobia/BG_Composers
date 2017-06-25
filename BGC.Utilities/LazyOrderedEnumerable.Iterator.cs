using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public partial class LazyOrderedEnumerable<T> : IOrderedEnumerable<T>
    {
        private class Iterator<TKey> : IEnumerator<T>
        {
            private static int RoundBase2Log(int number)
            {
                int maxBitIndex = 0;
                for (int i = 31; i > 0; i--)
                {
                    int mask = 1 << i;
                    if ((maxBitIndex & mask) != 0)
                    {
                        maxBitIndex = i;
                        break;
                    }
                }

                return maxBitIndex + 1;
            }

            private readonly Stack<IterationFrame> _sortingOps = new Stack<IterationFrame>();
            private readonly LazyOrderedEnumerable<T> _parent;
            private readonly List<T> _internalCollection;
            private readonly Func<T, TKey> _keySelector;
            private readonly IComparer<TKey> _keyComparer;
            private int _currentElementIndex;
            private int _currentIterationIndex;
            private int _maxSortedIndex;

            private void Swap(int xIndex, int yIndex)
            {
                T tmp = _internalCollection[xIndex];
                _internalCollection[xIndex] = _internalCollection[yIndex];
                _internalCollection[yIndex] = tmp;
            }

            private int ArrangeItems(IterationFrame frame)
            {
                int pivotIndex = frame.StartIndex + (frame.EndIndex - frame.StartIndex) / 2;
                for (int i = frame.EndIndex; pivotIndex < frame.EndIndex;)
                {
                    TKey pivotKey = _keySelector(_internalCollection[pivotIndex]);
                    TKey currElem = _keySelector(_internalCollection[i]);
                    if (_keyComparer.Compare(currElem, pivotKey) < 0)
                    {
                        Swap(i, pivotIndex + 1);
                        Swap(pivotIndex, pivotIndex + 1);
                        pivotIndex++;
                    }
                    else
                    {
                        i--;
                    }
                }

                for (int i = frame.StartIndex; i < pivotIndex;)
                {
                    TKey pivotKey = _keySelector(_internalCollection[pivotIndex]);
                    TKey currElem = _keySelector(_internalCollection[i]);
                    if (_keyComparer.Compare(currElem, pivotKey) > 0)
                    {
                        Swap(i, pivotIndex - 1);
                        Swap(pivotIndex, pivotIndex - 1);
                        pivotIndex--;
                    }
                    else
                    {
                        i++;
                    }
                }

                return pivotIndex;
            }

            private void TryPerformSortIteration()
            {
                while (_sortingOps.Count > 0  && _maxSortedIndex < _currentElementIndex)
                {
                    IterationFrame current = _sortingOps.Pop();
                    int pivotIndex = ArrangeItems(current);
                    _sortingOps.Push(current.Right(pivotIndex));
                    _sortingOps.Push(current.Left(pivotIndex));
                }
            }

            public Iterator(List<T> internalCollection)
            {
                _internalCollection = internalCollection;
                _sortingOps.Push(new IterationFrame(0, _internalCollection.Count - 1));
            }

            public T Current
            {
                get
                {
                    TryPerformSortIteration();

                    return _parent._internalCollection[_currentElementIndex];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                _currentElementIndex++;
                return _maxSortedIndex < _parent._internalCollection.Count - 1;
            }

            public void Reset()
            {
            }
        }
    }
}
