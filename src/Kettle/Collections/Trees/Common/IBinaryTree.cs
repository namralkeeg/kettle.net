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
using System.Collections;
using System.Collections.Generic;

namespace Kettle.Collections.Trees.Common
{
    /// <summary>
    /// Defines methods to manipulate a generic binary tree.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the binary tree.</typeparam>
    public interface IBinaryTree<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>
    {
        #region Properties

        /// <summary>
        /// Gets and sets the <see cref="IComparer{T}"/> used for comparing elements in a binary tree.
        /// </summary>
        /// <value>
        /// An <see cref="IComparer{T}"/> used for comparing elements in a binary tree.
        /// </value>
        IComparer<T> Comparer { get; set; }

        /// <summary>
        /// Gets the height of a binary tree.
        /// </summary>
        /// <value>The height of a binary tree is the number of edges between the tree's root and its furthest leaf.</value>
        int Height { get; }

        /// <summary>
        /// Gets the count of leaf nodes in the binary tree.
        /// </summary>
        /// <value>The total number of leaf nodes in the tree.</value>
        int LeafCount { get; }

        /// <summary>
        /// Gets the maximum width of a binary tree.
        /// </summary>
        /// <value>The maximum width of the tree at all levels.</value>
        int Width { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Traverses a binary tree Post-Order (LRN) and performs a callback <see cref="Action{T}"/> on each node.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each node.</param>
        void TraverseTreePostOrder(Action<T> action);

        /// <summary>
        /// Traverses a binary tree Pre-Order (NLR) and performs a callback <see cref="Action{T}"/> on each node.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each node.</param>
        void TraverseTreePreOrder(Action<T> action);

        #endregion Methods
    }
}