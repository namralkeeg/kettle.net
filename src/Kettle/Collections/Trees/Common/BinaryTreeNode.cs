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
    /// An abstract node for use in a binary tree.
    /// </summary>
    /// <typeparam name="TClass">
    /// The class type of the class deriving from this abstract base class to support circular
    /// generic references.
    /// </typeparam>
    /// <typeparam name="TData">The type of object stored in the <see cref="Value"/> property.</typeparam>
    internal abstract class BinaryTreeNode<TClass, TData> : BinaryNode<TClass, TData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        internal BinaryTreeNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(TData value) : base(value)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="left">The <see cref="Left"/> child node.</param>
        /// <param name="right">The <see cref="Right"/> child node.</param>
        /// <param name="value">The value to store in the node.</param>
        protected BinaryTreeNode(TClass left, TClass right, TData value) : base(left, right, value)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="parent"></param>
        protected BinaryTreeNode(TClass parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="parent">The <see cref="Parent"/> node.</param>
        /// <param name="value">The value to store in the node.</param>
        protected BinaryTreeNode(TData data, TClass parent) : base(data)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="left">The <see cref="Left"/> child node.</param>
        /// <param name="right">The <see cref="Right"/> child node.</param>
        /// <param name="value">The value to store in the node.</param>
        /// <param name="parent">The <see cref="Parent"/> node.</param>
        protected BinaryTreeNode(TClass left, TClass right, TData value, TClass parent) : base(left, right, value)
        {
            Parent = parent;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets and sets the parent node.
        /// </summary>
        internal virtual TClass Parent { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Resets everything to their default values.
        /// </summary>
        internal override void Invalidate()
        {
            base.Invalidate();
            Parent = default(TClass);
        }

        #endregion Methods
    }
}