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

namespace Kettle.Searching
{
    public sealed class LinearSearch<T> : SearchBase<T> where T : IComparable<T>
    {
        /// <summary>
        /// Initializes a <see cref="LinearSearch{T}"/> class.
        /// </summary>
        public LinearSearch()
        {
        }

        /// <summary>
        /// Initializes a <see cref="LinearSearch{T}"/> class.
        /// </summary>
        /// <param name="comparer">The custom comparer to use for searching.</param>
        public LinearSearch(IComparer<T> comparer) : base(comparer)
        {
        }

        /// <inheritdoc/>
        public override int Search(IList<T> list, T item, int start, int count, IComparer<T> comparer)
        {
            SearchValidationCheck(list, start, count);

            if (comparer == null)
            {
                comparer = _comparer;
            }

            int end = start + count;
            for (int i = start; i < end; i++)
            {
                if (comparer.Compare(item, list[i]) == 0)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}