using System.Collections.Generic;
using System.Linq;
using Kettle.Collections.Trees;
using Xunit;

namespace Kettle.Tests.Collections.Trees
{
    public class BinarySearchTreeTests
    {
        internal static int[] testArraySmall = { 1, 9, 2, 8, 3, 7, 4, 6, 5, 0, 10, 12, 14, 13, 11 };
        internal static int[] testArraySmallSorted = Enumerable.Range(0, 15).ToArray();
        internal static int[] testArrayMedium = { 1, 9, 2, 8, 18, 7, 19, 6, 5, 10, 0, 12, 14, 16, 3, 4, 17, 15, 11, 13 };
        internal static int[] testArrayMediumSorted = Enumerable.Range(0, 20).ToArray();

        internal class TreeCustomComparer : Comparer<int>
        {
            public override int Compare(int x, int y)
            {
                if (x < y)
                {
                    return -1;
                }
                else if (x == y)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        public class EmptyBinarySearchTree
        {
            private readonly BinarySearchTree<int> tree;

            public EmptyBinarySearchTree()
            {
                tree = new BinarySearchTree<int>();
            }

            [Fact]
            public void Count_ShouldReturnZero()
            {
                int treeCount = tree.Count;
                Assert.Equal(0, treeCount);
            }

            [Fact]
            public void Contains_ShouldReturnFalse()
            {
                bool contains = tree.Contains(10);

                Assert.False(contains);
            }

            [Fact]
            public void Remove_ShouldReturnFalse()
            {
                bool removed = tree.Remove(10);
                Assert.False(removed);
            }

            [Fact]
            public void Height_ShouldBeNegative()
            {
                int height = tree.Height;
                Assert.Equal(-1, height);
            }

            [Fact]
            public void LeafCount_ShouldBeZero()
            {
                int leafCount = tree.LeafCount;
                Assert.Equal(0, leafCount);
            }

            [Fact]
            public void Width_ShouldBeZero()
            {
                int width = tree.Width;
                Assert.Equal(0, width);
            }
        }

        public class BinarySearchTreeOneElement
        {
            private const int oneValue = 42;
            private readonly BinarySearchTree<int> tree;

            public BinarySearchTreeOneElement()
            {
                tree = new BinarySearchTree<int>
                {
                    oneValue
                };
            }

            [Fact]
            public void Count_ShouldBeOne()
            {
                Assert.Single(tree);
            }

            [Fact]
            public void Contains_ShouldBeTrue()
            {
                bool contains = tree.Contains(oneValue);
                Assert.True(contains);
            }

            [Fact]
            public void Remove_ShouldReturnTrue()
            {
                bool removed = tree.Remove(oneValue);
                Assert.True(removed);
            }

            [Fact]
            public void Remove_CountShouldBeZero()
            {
                tree.Remove(oneValue);
                int count = tree.Count;
                Assert.Equal(0, count);
            }

            [Fact]
            public void Height_ShouldBeZero()
            {
                int height = tree.Height;
                Assert.Equal(0, height);
            }

            [Fact]
            public void LeafCount_ShouldBeOne()
            {
                Assert.Equal(1, tree.LeafCount);
            }

            [Fact]
            public void Width_ShouldBeZero()
            {
                Assert.Equal(0, tree.Width);
            }
        }

        public class BinarySearchTreeMultipleValues
        {
            private static readonly int[] unsortedArray = { 11, 6, 8, 19, 4, 10, 5, 17, 43, 49, 31 };
            private static readonly int[] sortedArray = { 4, 5, 6, 8, 10, 11, 17, 19, 31, 43, 49 };
            private readonly BinarySearchTree<int> tree;

            public BinarySearchTreeMultipleValues()
            {
                tree = new BinarySearchTree<int>();
                foreach (var item in unsortedArray)
                {
                    tree.Add(item);
                }
            }

            [Fact]
            public void Count_ShouldBeEleven()
            {
                Assert.Equal(11, tree.Count);
            }

            [Fact]
            public void Contains_ShouldBeTrue()
            {
                bool contains = tree.Contains(unsortedArray[10]);
                Assert.True(contains);
            }

            [Fact]
            public void Contains_EnumerableShouldBeTrue()
            {
                Assert.Contains(unsortedArray[10], tree);
            }

            [Fact]
            public void Remove_CountShouldBeTen()
            {
                tree.Remove(unsortedArray[7]);
                Assert.Equal(10, tree.Count);
            }

            [Fact]
            public void Height_ShouldBeThree()
            {
                Assert.Equal(3, tree.Height);
            }

            [Fact]
            public void LeafCount_ShouldBeFive()
            {
                Assert.Equal(5, tree.LeafCount);
            }

            [Fact]
            public void Width_ShouldBeFour()
            {
                Assert.Equal(4, tree.Width);
            }

            [Fact]
            public void Enumerate_VerifyInOrder()
            {
                int i = 0;
                foreach (var item in tree)
                {
                    Assert.Equal(sortedArray[i++], item);
                }
            }

            [Fact]
            public void Enumerate_VerifyReverseOrder()
            {
                int i = tree.Count - 1;
                using (var enumerator = tree.GetReverseEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Assert.Equal(sortedArray[i--], enumerator.Current);
                    }
                }
            }
        }
    }
}