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

namespace Kettle.Collections.Trees.Common
{
    /// <summary>
    /// An abstract binary tree node class with a key.
    /// </summary>
    /// <typeparam name="TClass">
    /// The class type of the class deriving from this abstract base class to support circular
    /// generic references.
    /// </typeparam>
    /// <typeparam name="TKey">The type of object stored in the <see cref="Key"/> property.</typeparam>
    /// <typeparam name="TData">The type of object stored in the <see cref="Value"/> property.</typeparam>
    internal abstract class KeyedBinaryTreeNode<TClass, TKey, TData> : KeyedBinaryNode<TClass, TKey, TData>
    {
        /// <summary>
        /// Initializes a <see cref="KeyedBinaryTreeNode{TClass, TKey, TData}"/> class.
        /// </summary>
        protected KeyedBinaryTreeNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryTreeNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="key">They key to store in the node.</param>
        protected KeyedBinaryTreeNode(TKey key) : base(key)
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryTreeNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="key">They key to store in the node.</param>
        /// <param name="value">The value to store in the node.</param>
        protected KeyedBinaryTreeNode(TKey key, TData value) : base(key, value)
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryTreeNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="key">They key to store in the node.</param>
        /// <param name="value">The value to store in the node.</param>
        /// <param name="parent"></param>
        protected KeyedBinaryTreeNode(TKey key, TData value, TClass parent) : base(key, value)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryTreeNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        /// <param name="value">The value to store in the node.</param>
        /// <param name="key">They key to store in the node.</param>
        protected KeyedBinaryTreeNode(TClass left, TClass right, TData value, TKey key) : base(left, right, value, key)
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryTreeNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        /// <param name="value">The value to store in the node.</param>
        /// <param name="key">They key to store in the node.</param>
        /// <param name="parent"></param>
        protected KeyedBinaryTreeNode(TClass left, TClass right, TData value, TKey key, TClass parent) 
            : base(left, right, value, key)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets and sets the parent node.
        /// </summary>
        internal virtual TClass Parent { get; set; }

        /// <inheritdoc/>
        internal override void Invalidate()
        {
            base.Invalidate();
            Parent = default(TClass);
        }
    }
}
