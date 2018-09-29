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

namespace Kettle.Sorting
{
    /// <summary>
    /// A Cycle Sort implementation of the <see cref="ICompareSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class CycleSort<T> : CompareSort<T> where T : IComparable<T>
    {
        /// <summary>
        /// Initializes a <see cref="CycleSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public CycleSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="CycleSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public CycleSort(IComparer<T> comparer) : base(comparer)
        {
        }

        /// <inheritdoc/>
        public override void Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int end = start + count;
            for (int cyclePosition = start; cyclePosition < end; cyclePosition++)
            {
                T item = list[cyclePosition];
                int position = cyclePosition;
                for (int i = cyclePosition + 1; i < end; i++)
                {
                    if (Comparer.Compare(list[i], item) < 0)
                    {
                        position++;
                    }
                }

                if (position == cyclePosition)
                {
                    continue;
                }

                while (Comparer.Compare(item, list[position]) == 0)
                {
                    position++;
                }

                if (position != cyclePosition)
                {
                    T temp = item;
                    item = list[position];
                    list[position] = temp;
                }

                while (position != cyclePosition)
                {
                    position = cyclePosition;
                    for (int i = cyclePosition + 1; i < count; i++)
                    {
                        if (Comparer.Compare(list[i], item) < 0)
                        {
                            position++;
                        }
                    }

                    while (Comparer.Compare(item, list[position]) == 0)
                    {
                        position++;
                    }

                    if (Comparer.Compare(item, list[position]) != 0)
                    {
                        T temp = item;
                        item = list[position];
                        list[position] = temp;
                    }
                }
            }
        }
    }
}