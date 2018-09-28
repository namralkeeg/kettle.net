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

using System.Collections.Generic;

namespace Kettle.Sorting
{
    /// <summary>
    /// Represents a set functions for sorting a generic <see cref="IList{T}"/> of objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects in the <see cref="IList{T}"/> to sort.
    /// </typeparam>
    public interface ISort<T>
    {
        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        void Sort(IList<T> list);

        /// <summary>
        /// Sorts the <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to sort.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        void Sort(IList<T> list, int start, int count);
    }
}