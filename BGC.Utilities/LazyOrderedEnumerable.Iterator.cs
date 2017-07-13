using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public partial class LazyOrderedEnumerable<T, TKey> : IOrderedEnumerable<T>
    {
        private class Iterator<T> : IEnumerator<T>
        {
            private readonly Stack<IterationFrame> _sortingOps = new Stack<IterationFrame>();
            private readonly LazyOrderedEnumerable<T, TKey> _parent;
            private readonly List<T> _internalCollection;
            private int _currentElementIndex = -1;
            private int _maxSortedIndex = -1;

            private void Swap(int xIndex, int yIndex)
            {
                T tmp = _internalCollection[xIndex];
                _internalCollection[xIndex] = _internalCollection[yIndex];
                _internalCollection[yIndex] = tmp;
            }

            private int ArrangeItems(IterationFrame frame)
            {
                int pivotIndex = frame.StartIndex + (frame.EndIndex - frame.StartIndex) / 2;
                for (int i = frame.EndIndex; i > pivotIndex;)
                {
                    if (_parent.Compare(_internalCollection[i], _internalCollection[pivotIndex]) < 0)
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
                    if (_parent.Compare(_internalCollection[i], _internalCollection[pivotIndex]) > 0)
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

            private void EnsureHasNextSortedElement()
            {
                while (_sortingOps.Count > 0  && _maxSortedIndex < _currentElementIndex)
                {
                    IterationFrame current = _sortingOps.Pop();

                    if (current.ElementsCount > 2)
                    {
                        int pivotIndex = ArrangeItems(current);
                        _sortingOps.Push(current.Right(pivotIndex));
                        _sortingOps.Push(current.Left(pivotIndex));
                    }
                    else
                    {
                        if (current.ElementsCount == 2 && _parent.Compare(_internalCollection[current.StartIndex], _internalCollection[current.EndIndex]) > 0)
                        {
                            Swap(current.StartIndex, current.EndIndex);
                        }
                        _maxSortedIndex = current.EndIndex;
                    }
                }
            }

            public Iterator(List<T> internalCollection, LazyOrderedEnumerable<T, TKey> parent)
            {
                _internalCollection = internalCollection;
                _parent = parent;
                _sortingOps.Push(new IterationFrame(0, _internalCollection.Count - 1));
            }

            public T Current
            {
                get
                {
                    EnsureHasNextSortedElement();

                    return _parent._internalCollection[_currentElementIndex];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return _currentElementIndex++ < _parent._internalCollection.Count - 1;
            }

            public void Reset()
            {
                _currentElementIndex = -1;
            }
        }
    }
}
