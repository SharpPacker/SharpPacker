using BoxPackerClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BoxPackerClone.Tests
{
    public class ItemListTest
    {
        [Fact]
        public void TestCount()
        {
            var itemList = new ItemList();
            Assert.Equal(0, itemList.Count);

            itemList.Insert(Factory.CreateItem("Item A", 20, 20, 2, 100, true));
            Assert.Equal(1, itemList.Count);

            itemList.Insert(Factory.CreateItem("Item B", 20, 20, 2, 100, true));
            Assert.Equal(2, itemList.Count);

            itemList.Insert(Factory.CreateItem("Item C", 20, 20, 2, 100, true));
            Assert.Equal(3, itemList.Count);

            var deleted = itemList.Extract();
            Assert.Equal(2, itemList.Count);
        }

        /// <summary>
        /// Test that sorting of items with different dimensions works as expected i.e.
        /// * - Largest(by volume) first
        /// * - If identical volume, sort by weight.
        /// </summary>
        [Fact]
        public void TestDimensionalSorting()
        {
            var item1 = Factory.CreateItem("A1_Small", 20, 20, 2, 100, true);
            var item2 = Factory.CreateItem("C2_Large", 200, 200, 20, 1000, true);
            var item3 = Factory.CreateItem("B3_Medium", 100, 100, 10, 500, true);
            var item4 = Factory.CreateItem("D4_Medium Heavy", 100, 100, 10, 501, true);

            var list = new ItemList();
            list.Insert(item1);
            list.Insert(item2);
            list.Insert(item3);
            list.Insert(item4);

            var sorted = list.ToList();

            // TODO Check if this test is OK in original library - looks like in current ItemList
            // sorting is inverted:
            // -- Dough: Reverse internal ordering of ItemList so that it can use O(1) array_pop
            //    rather than O(n) array_shift
            Assert.Equal(item2.Description, sorted[0].Description);
            Assert.Equal(item4.Description, sorted[1].Description);
            Assert.Equal(item3.Description, sorted[2].Description);
            Assert.Equal(item1.Description, sorted[3].Description);
        }

        /// <summary>
        /// Test we can retrieve the "top" (next) item in the list.
        /// </summary>
        [Fact]
        public void TestExtract()
        {
            var itemList = new ItemList();

            var item1 = Factory.CreateItem("Item A", 20, 20, 2, 100, true);
            itemList.Insert(item1);

            Assert.Equal(item1.Description, itemList.Extract().Description);
            Assert.Equal(0, itemList.Count);
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

            var list = new ItemList();
            list.Insert(item1);
            list.Insert(item2);
            list.Insert(item3);
            list.Insert(item4);

            var sorted = list.ToList();

            // TODO Check if this test is OK in original library - looks like in current ItemList
            // sorting is inverted:
            // -- Dough: Reverse internal ordering of ItemList so that it can use O(1) array_pop
            //    rather than O(n) array_shift
            Assert.Equal(sorted[0].Description, sorted[1].Description);
            Assert.Equal(sorted[2].Description, sorted[3].Description);

            Assert.Equal(item1.Description, sorted[0].Description);
            Assert.Equal(item3.Description, sorted[1].Description);
            Assert.Equal(item2.Description, sorted[2].Description);
            Assert.Equal(item4.Description, sorted[3].Description);
        }

        /// <summary>
        /// Test we can peek at the "top" (next) item in the list.
        /// </summary>
        [Fact]
        public void TestTop()
        {
            var itemList = new ItemList();

            var item1 = Factory.CreateItem("Item A", 20, 20, 2, 100, true);
            itemList.Insert(item1);

            Assert.Equal(item1.Description, itemList.Top().Description);
            Assert.Equal(1, itemList.Count);
        }

        /// <summary>
        /// Test that we can retrieve an accurate count of items in the list.
        /// </summary>
        [Fact]
        public void TestTopN()
        {
            var itemList = new ItemList();

            var item1 = Factory.CreateItem("Item A", 20, 20, 2, 100, true);
            itemList.Insert(item1);

            var item2 = Factory.CreateItem("Item B", 20, 20, 2, 100, true);
            itemList.Insert(item2);

            var item3 = Factory.CreateItem("Item C", 20, 20, 2, 100, true);
            itemList.Insert(item3);

            var top2 = itemList.TopN(2);

            Assert.Equal(2, top2.Count);
            Assert.Equal(item1.Description, top2.Extract().Description);
            Assert.Equal(item2.Description, top2.Extract().Description);
        }
    }
}

