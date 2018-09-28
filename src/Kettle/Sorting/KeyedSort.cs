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

namespace Kettle.Sorting
{
    /// <summary>
    /// Provides a base class for implementing the <see cref="IKeyedSort{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort.</typeparam>
    /// <remarks>
    /// Key based sorting is a non-comparison based sort, and instead relies on a key function.
    /// </remarks>
    public abstract class KeyedSort<T> : SortBase<T>, IKeyedSort<T>
    {
        #region Fields

        protected Func<T, int> _keyFunction;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="KeyedSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the key function to <see cref="object.GetHashCode"/>.
        /// </remarks>
        protected KeyedSort() : this((T k) => k.GetHashCode())
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedSort{T}"/> class.
        /// </summary>
        /// <param name="keyFunction">
        /// A function <see cref="Func{T, int}"/> that returns an int key based on the value of T.
        /// </param>
        protected KeyedSort(Func<T, int> keyFunction)
        {
            KeyFunction = keyFunction;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Func{T, int}"/> to use for generating keys.
        /// </summary>
        /// <remarks>Defaults to <see cref="object.GetHashCode"/> for key generation if null.</remarks>
        public virtual Func<T, int> KeyFunction
        {
            get => _keyFunction;
            set
            {
                _keyFunction = value ?? ((T k) => k.GetHashCode());
            }
        }

        #endregion Properties
    }
}