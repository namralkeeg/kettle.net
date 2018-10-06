#region Copyright

/*
 * Copyright (C) 2018 Larry Lopez
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */

#endregion Copyright

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kettle.Common;

namespace Kettle.Sorting
{
    /// <summary>
    ///  A Quicksort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class QuickSort<T> : CompareSort<T> where T : IComparable<T>
    {
        #region Fields

        private const int _DefaultThreshold = 16;
        private const string _RecursiveCuttoffMessage = "Value must be greater than zero.";
        private const string _ThresholdMessage = "Threshold must be greater than 2.";

        private static readonly int _maxParallelTasks = Environment.ProcessorCount * 2;
        private volatile int _parallelInvokeCalls;
        private bool _sorting;
        private int _threshold;
        private ICompareSort<T> _thresholdSort;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="QuickSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/> which should
        /// be the <see cref="IComparable{T}"/> implementation for T. Defaults the alternate sorting
        /// algorithm to <see cref="InsertionSort{T}"/>.
        /// </remarks>
        public QuickSort()
        {
            InParallel = false;
            _parallelInvokeCalls = 0;
            _sorting = false;
            _threshold = _DefaultThreshold;
            _thresholdSort = new InsertionSort<T>(_comparer);
        }

        /// <summary>
        /// Initializes a <see cref="QuickSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        /// <remarks>Defaults the alternate sorting algorithm to <see cref="InsertionSort{T}"/>.</remarks>
        public QuickSort(IComparer<T> comparer) : base(comparer)
        {
            InParallel = false;
            _parallelInvokeCalls = 0;
            _sorting = false;
            _threshold = _DefaultThreshold;
            _thresholdSort = new InsertionSort<T>(_comparer);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets and sets the value for if the sort algorithm will be run in parallel or not.
        /// </summary>
        public bool InParallel { get; set; }

        /// <summary>
        /// Gets and sets the threshold count for when to use the alternate sorting algorithm.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is less than 2.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when changing the value while sorting is in progress.
        /// </exception>
        public int Threshold
        {
            get => _threshold;
            set
            {
                if (_sorting)
                {
                    throw new InvalidOperationException("Unable to change threshold while sorting.");
                }

                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _ThresholdMessage);
                }

                _threshold = value;
            }
        }

        /// <summary>
        /// Gets and sets the alternate sorting algorithm to use when the <see cref="Threshold"/> is reached.
        /// </summary>
        /// <remarks>On null it defaults to <see cref="InsertionSort{T}"/>.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when changing the value while sorting is in progress.
        /// </exception>
        public ICompareSort<T> ThresholdSort
        {
            get => _thresholdSort;
            set
            {
                if (_sorting)
                {
                    throw new InvalidOperationException("Unable to change threshold sort algorithm while sorting.");
                }

                _thresholdSort = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            _sorting = true;
            int last = start + count - 1;

            // If the number of items is below the threshold, then use the alternate sorting algorithm.
            // Defaults to Insertion Sort.
            if (count <= Threshold)
            {
                ThresholdSort.Sort(list, start, count);
            }

            if (InParallel)
            {
                QuickSortInternalParallel(list, start, last);
            }
            else
            {
                QuickSortInternal(list, start, last);
            }

            _sorting = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        /// <returns></returns>
        private int Partition(IList<T> list, int low, int high)
        {
            // Begin median-of-three pivot algorithm.
            // https://en.wikipedia.org/wiki/Quicksort#Choice_of_pivot
            int middle = low + ((high - low) >> 1); // Same as (low + high) / 2, but prevents an integer overflow.

            if (_comparer.Compare(list[middle], list[low]) < 0)
            {
                Utils.Swap(list, low, middle);
            }

            if (_comparer.Compare(list[high], list[low]) < 0)
            {
                Utils.Swap(list, low, high);
            }

            if (_comparer.Compare(list[high], list[middle]) < 0)
            {
                Utils.Swap(list, middle, high);
            }

            // Begin partitioning.
            T pivotValue = list[middle];

            // Place the pivot at end - 1.
            Utils.Swap(list, middle, high - 1);

            // Low and High are already partitioned relative to the pivot in median-of-three.
            int i = low + 1;
            int j = high - 2;

            while (true)
            {
                while (_comparer.Compare(list[i], pivotValue) < 0)
                {
                    i++;
                }

                while (_comparer.Compare(list[j], pivotValue) > 0)
                {
                    j--;
                }

                if (i >= j)
                {
                    // Return the pivot back to the middle.
                    // Everything is now less than the pivot on the left and greater than on the right.
                    Utils.Swap(list, i, high - 1);
                    return i;
                }

                Utils.Swap(list, i, j);
            }
        }

        /// <summary>
        /// Internal Quicksort sequential recursive sorting function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        private void QuickSortInternal(IList<T> list, int low, int high)
        {
            // If the number of items left is below the threshold, use the alternate sort.
            // Default is Insertion Sort.
            int count = high - low + 1;
            if (count <= Threshold)
            {
                ThresholdSort.Sort(list, low, count);
            }
            else
            {
                int partitionIndex = Partition(list, low, high);
                QuickSortInternal(list, low, partitionIndex - 1);
                QuickSortInternal(list, partitionIndex + 1, high);
            }
        }

        /// <summary>
        /// Internal Quicksort parallel recursive sorting function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        private void QuickSortInternalParallel(IList<T> list, int low, int high)
        {
            // If the number of items left is below the threshold, use the alternate sort.
            // Default is Insertion Sort.
            int count = high - low + 1;
            if (count <= Threshold)
            {
                ThresholdSort.Sort(list, low, count);
            }
            else
            {
                int partitionIndex = Partition(list, low, high);
                if (_parallelInvokeCalls < _maxParallelTasks)
                {
                    Interlocked.Increment(ref _parallelInvokeCalls);
                    Parallel.Invoke(
                        () => QuickSortInternal(list, low, partitionIndex - 1),
                        () => QuickSortInternal(list, partitionIndex + 1, high));
                    Interlocked.Decrement(ref _parallelInvokeCalls);
                }
                else
                {
                    QuickSortInternal(list, low, partitionIndex - 1);
                    QuickSortInternal(list, partitionIndex + 1, high);
                }
            }
        }

        #endregion Methods
    }
}