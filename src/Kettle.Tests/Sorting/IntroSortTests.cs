using System;
using System.Collections.Generic;
using System.Linq;
using Kettle.Sorting;
using Xunit;

namespace Kettle.Tests.Sorting
{
    public class IntroSortTests
    {
        internal static int[] testArraySmall = { 1, 9, 2, 8, 3, 7, 4, 6, 5, 0, 10, 12, 14, 13, 11 };
        internal static int[] testArrayMedium = { 1, 9, 2, 8, 18, 7, 19, 6, 5, 10, 0, 12, 14, 16, 3, 4, 17, 15, 11, 13 };
        internal static ISort<int> defaultSort = new IntroSort<int>();
        internal static ISort<int> customSort = new IntroSort<int>(new SortCustomComparer());

        internal class SortCustomComparer : Comparer<int>
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

        [Fact]
        public void Sort_AlternateSortEntireListDefaultCompare()
        {
            var tempArray = (int[])testArraySmall.Clone();
            defaultSort.Sort(tempArray);

            for (int i = 0; i < tempArray.Length; i++)
            {
                Assert.Equal(i, tempArray[i]);
            }
        }

        [Fact]
        public void Sort_AlternateSortEntireListCustomCompare()
        {
            var tempArray = (int[])testArraySmall.Clone();
            customSort.Sort(tempArray);

            for (int i = 0; i < tempArray.Length; i++)
            {
                Assert.Equal(i, tempArray[i]);
            }
        }

        [Fact]
        public void Sort_SortEntireListDefaultCompare()
        {
            var tempArray = (int[])testArrayMedium.Clone();
            defaultSort.Sort(tempArray);

            for (int i = 0; i < tempArray.Length; i++)
            {
                Assert.Equal(i, tempArray[i]);
            }
        }

        [Fact]
        public void Sort_SortEntireListCustomCompare()
        {
            var tempArray = (int[])testArrayMedium.Clone();
            customSort.Sort(tempArray);

            for (int i = 0; i < tempArray.Length; i++)
            {
                Assert.Equal(i, tempArray[i]);
            }
        }

        [Fact]
        public void Sort_StressTestDefaultCompare()
        {
            var random = new Random();
            int nodes = 10000;
            var randomList = Enumerable.Range(0, nodes)
                .OrderBy(x => random.Next())
                .ToArray();

            defaultSort.Sort(randomList);
            for (int i = 0; i < nodes; i++)
            {
                Assert.Equal(i, randomList[i]);
            }
        }

        [Fact]
        public void Sort_StressTestCustomCompare()
        {
            var random = new Random();
            int nodes = 10000;
            var randomList = Enumerable.Range(0, nodes)
                .OrderBy(x => random.Next())
                .ToArray();

            customSort.Sort(randomList);
            for (int i = 0; i < nodes; i++)
            {
                Assert.Equal(i, randomList[i]);
            }
        }

        [Fact]
        public void Sort_WorstCaseRecursionDepth()
        {
            int nodes = 10000;
            var badList = Enumerable.Repeat(1, nodes).ToArray();

            defaultSort.Sort(badList, 0, nodes);
            for (int i = 0; i < nodes; i++)
            {
                Assert.Equal(1, badList[i]);
            }
        }
    }
}