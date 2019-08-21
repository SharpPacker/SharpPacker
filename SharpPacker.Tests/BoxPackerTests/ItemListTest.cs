using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpPacker;
using SharpPacker.Models;
using System.Linq;

namespace SharpPacker.Tests.BoxPackerTests
{
    public class ItemListTest
    {
        /// <summary>
        /// Test that sorting of items with different dimensions works as expected i.e.
        /// * - Largest(by volume) first
        /// * - If identical volume, sort by weight.
        /// </summary>
        [Fact]
        public void TestDimensionalSorting()
        {
            var item1 = Factory.CreateItem("A_Small", 20, 20, 2, 100, true);
            var item2 = Factory.CreateItem("C_Large", 200, 200, 20, 1000, true);
            var item3 = Factory.CreateItem("B_Medium", 100, 100, 10, 500, true);
            var item4 = Factory.CreateItem("D_Medium Heavy", 100, 100, 10, 501, true);

            var list = new List<Item4d>
            {
                item1,
                item2,
                item3,
                item4
            };

            var sorted = (list as IEnumerable<Item4d>).OrderBy(item => item).ToList();

            // TODO Check if this test is OK in original library - looks like in current ItemList sorting is inverted: 
            // -- Dough: Reverse internal ordering of ItemList so that it can use O(1) array_pop rather than O(n) array_shift
            Assert.Equal(item2.Description, sorted[0].Description);
            Assert.Equal(item4.Description, sorted[1].Description);
            Assert.Equal(item3.Description, sorted[2].Description);
            Assert.Equal(item1.Description, sorted[3].Description);
        }

        /// <summary>
        /// Test that sorting of items with identical dimensions works as expected i.e.
        /// * - Items with the same name(i.e.same type) are kept together.
        /// </summary>
        [Fact]
        public void TestKeepingItemsOfSameTypeTogether()
        {
            var item1 = Factory.CreateItem("Item A", 20, 20, 2, 100, true);
            var item2 = Factory.CreateItem("Item B", 20, 20, 2, 100, true);
            var item3 = Factory.CreateItem("Item A", 20, 20, 2, 100, true);
            var item4 = Factory.CreateItem("Item B", 20, 20, 2, 100, true);

            var list = new List<Item4d>
            {
                item1,
                item2,
                item3,
                item4
            };

            var sorted = (list as IEnumerable<Item4d>).OrderBy(item => item).ToList();

            // TODO Check if this test is OK in original library - looks like in current ItemList sorting is inverted: 
            // -- Dough: Reverse internal ordering of ItemList so that it can use O(1) array_pop rather than O(n) array_shift
            Assert.Equal(item1.Description, sorted[0].Description);
            Assert.Equal(item3.Description, sorted[1].Description);
            Assert.Equal(item2.Description, sorted[2].Description);
            Assert.Equal(item4.Description, sorted[3].Description);
        }

        [Fact]
        public void TestCount()
        {
            throw new NotImplementedException();
        }
        [Fact]
        public void TestTop()
        {
            throw new NotImplementedException();
        }
        [Fact]
        public void TestExtract()
        {
            throw new NotImplementedException();
        }
    }
}
