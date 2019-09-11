using BoxPackerClone.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BoxPackerClone.Tests.BoxPackerTests
{
    public class BoxListTest
    {
        /// <summary>
        /// Test that sorting of boxes with identical dimensions works as expected i.e. order by
        /// maximum weight capacity.
        /// </summary>
        [Fact]
        public void TestIssue163()
        {
            var box2 = Factory.CreateBox("C2", 202, 152, 32, 10, 200, 150, 30, 100);
            var box3 = Factory.CreateBox("B3", 202, 152, 32, 10, 200, 150, 30, 250);
            var box1 = Factory.CreateBox("A1", 202, 152, 32, 10, 200, 150, 30, 50);

            var list = new BoxList();
            list.Insert(box1);
            list.Insert(box2);
            list.Insert(box3);

            var sorted = list.ToList();

            Assert.Equal(box1.Reference, sorted[0].Reference);
            Assert.Equal(box2.Reference, sorted[1].Reference);
            Assert.Equal(box3.Reference, sorted[2].Reference);
        }

        /// <summary>
        /// Test that items with a volume greater than 2^31-1 are sorted correctly.
        /// </summary>
        [Fact]
        public void TestIssue30A()
        {
            var box1 = Factory.CreateBox("C_Small_1", 21, 21, 3, 1, 20, 20, 2, 100);
            var box2 = Factory.CreateBox("B_Large_2", 1301, 1301, 1301, 1, 1300, 1300, 1300, 1000);
            var box3 = Factory.CreateBox("A_Medium_3", 101, 101, 11, 5, 100, 100, 10, 500);

            var list = new BoxList();
            list.Insert(box1);
            list.Insert(box2);
            list.Insert(box3);

            var sorted = list.ToList();

            Assert.Equal(box1.Reference, sorted[0].Reference);
            Assert.Equal(box3.Reference, sorted[1].Reference);
            Assert.Equal(box2.Reference, sorted[2].Reference);
        }

        /// <summary>
        /// Test that items with a volume greater than 2^31-1 are sorted correctly.
        /// </summary>
        [Fact]
        public void TestIssue30B()
        {
            var box1 = Factory.CreateBox("C_Small_1", 21, 21, 3, 1, 20, 20, 2, 100);
            var box2 = Factory.CreateBox("B_Large_2", 1301, 1301, 1301, 1, 1300, 1300, 1300, 1000);
            var box3 = Factory.CreateBox("A_Medium_3", 101, 101, 11, 5, 100, 100, 10, 500);

            var list1 = new BoxList();
            list1.Insert(box1);
            list1.Insert(box2);
            list1.Insert(box3);
            var sorted1 = list1.ToList();

            Assert.Equal(box1.Reference, sorted1[0].Reference);
            Assert.Equal(box3.Reference, sorted1[1].Reference);
            Assert.Equal(box2.Reference, sorted1[2].Reference);

            var list2 = new BoxList();
            list2.Insert(box1);
            list2.Insert(box2);
            list2.Insert(box3);
            var sorted2 = list1.ToList();

            Assert.Equal(box1.Reference, sorted1[0].Reference);
            Assert.Equal(box3.Reference, sorted1[1].Reference);
            Assert.Equal(box2.Reference, sorted1[2].Reference);
        }

        /// <summary>
        /// Test that sorting of boxes with different dimensions works as expected i.e. Largest(by
        /// volume) first.
        /// </summary>
        [Fact]
        public void TestSorting()
        {
            var box1 = Factory.CreateBox("C_Small", 21, 21, 3, 1, 20, 20, 2, 100);
            var box2 = Factory.CreateBox("B_Large", 201, 201, 21, 1, 200, 200, 20, 1000);
            var box3 = Factory.CreateBox("A_Medium", 101, 101, 11, 5, 100, 100, 10, 500);

            var list = new BoxList();
            list.Insert(box1);
            list.Insert(box2);
            list.Insert(box3);

            var sorted = list.ToList();

            Assert.Equal("C_Small", sorted[0].Reference);
            Assert.Equal("A_Medium", sorted[1].Reference);
            Assert.Equal("B_Large", sorted[2].Reference);
        }
    }
}
