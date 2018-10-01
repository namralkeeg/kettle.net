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

namespace Kettle.Searching
{
    /// <summary>
    /// Represents a set of functions for searching a generic <see cref="IList{T}"/> of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects in the <see cref="IList{T}"/> to search.</typeparam>
    public interface ISearch<T>
    {
        /// <summary>
        /// Performs a search on the entire <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        int Search(IList<T> list, T item);

        /// <summary>
        /// Performs a search on the entire <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="comparer">The custom comparer to use.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        int Search(IList<T> list, T item, IComparer<T> comparer);

        /// <summary>
        /// Performs a search on a section of the <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        int Search(IList<T> list, T item, int start, int count);

        /// <summary>
        /// Performs a search on a section of the <see cref="IList{T}"/> of objects.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <param name="comparer">The custom comparer to use.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        int Search(IList<T> list, T item, int start, int count, IComparer<T> comparer);
    }
}