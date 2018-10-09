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
    /// An abstract binary node base class.
    /// </summary>
    /// <typeparam name="TClass">
    /// The class type of the class deriving from this abstract base class to support circular
    /// generic references.
    /// </typeparam>
    /// <typeparam name="TData">The type of object stored in the <see cref="Value"/> property.</typeparam>
    internal abstract class BinaryNode<TClass, TData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="BinaryNode{TClass, TData}"/> class.
        /// </summary>
        protected BinaryNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        protected BinaryNode(TData value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a <see cref="BinaryNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="left">The <see cref="Left"/> node.</param>
        /// <param name="right">The <see cref="Right"/> node.</param>
        /// <param name="value">The value to store in the node.</param>
        protected BinaryNode(TClass left, TClass right, TData value)
        {
            Left = left;
            Right = right;
            Value = value;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets if this node is a leaf node.
        /// </summary>
        internal virtual bool IsLeaf => (Left == null) && (Right == null);

        /// <summary>
        /// Gets and sets reference to the left node.
        /// </summary>
        internal virtual TClass Left { get; set; }

        /// <summary>
        /// Gets and sets reference to the right node.
        /// </summary>
        internal virtual TClass Right { get; set; }

        /// <summary>
        /// Gets and sets the value to store in the node.
        /// </summary>
        internal virtual TData Value { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Resets everything to their default values.
        /// </summary>
        internal virtual void Invalidate()
        {
            Left = default(TClass);
            Right = default(TClass);
            Value = default(TData);
        }

        #endregion Methods
    }
}