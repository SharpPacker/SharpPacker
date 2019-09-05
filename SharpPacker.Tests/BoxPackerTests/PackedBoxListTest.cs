using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SharpPacker.Tests.BoxPackerTests
{
    public class PackedBoxListTest
    {
        /// <summary>
        /// Test that inserting individually correctly works.
        /// </summary>
        [Fact]
        public void TestInsertAndCount()
        {
            var box = Factory.CreateBox("Box", 10, 10, 10, 0, 10, 10, 10, 100);
            var itemA = Factory.CreateItem("Item A", 5, 10, 10, 10, true);
            var itemB = Factory.CreateItem("Item B", 5, 10, 10, 20, true);

            var packedItemA = new PackedItem(itemA, 0, 0, 0, 5, 10, 10);
            var packedItemB = new PackedItem(itemB, 0, 0, 0, 5, 10, 10);

            var packedItemListA = new PackedItemList();
            packedItemListA.Insert(packedItemA);
            var packedBoxA = new PackedBox(box, packedItemListA);

            var packedItemListB = new PackedItemList();
            packedItemListB.Insert(packedItemB);
            var packedBoxB = new PackedBox(box, packedItemListB);

            var packedBoxList = new PackedBoxList();
            packedBoxList.Insert(packedBoxA);
            packedBoxList.Insert(packedBoxB);

            Assert.Equal(2, packedBoxList.Count);
        }

        /// <summary>
        /// Test that inserting in bulk correctly works.
        /// </summary>
        [Fact]
        public void TestInsertFromArrayAndCount()
        {
            var box = Factory.CreateBox("Box", 10, 10, 10, 0, 10, 10, 10, 100);
            var itemA = Factory.CreateItem("Item A", 5, 10, 10, 10, true);
            var itemB = Factory.CreateItem("Item B", 5, 10, 10, 20, true);

            var packedItemA = new PackedItem(itemA, 0, 0, 0, 5, 10, 10);
            var packedItemB = new PackedItem(itemB, 0, 0, 0, 5, 10, 10);

            var packedItemListA = new PackedItemList();
            packedItemListA.Insert(packedItemA);
            var packedBoxA = new PackedBox(box, packedItemListA);

            var packedItemListB = new PackedItemList();
            packedItemListB.Insert(packedItemB);
            var packedBoxB = new PackedBox(box, packedItemListB);

            var pBoxArray = new PackedBox[] { packedBoxA, packedBoxB };

            var packedBoxList = new PackedBoxList();
            packedBoxList.InsertFromArray(pBoxArray);

            Assert.Equal(2, packedBoxList.Count);
        }

        /// <summary>
        /// Test we can peek at the "top" (next) item in the list.
        /// </summary>
        [Fact]
        public void TestTop()
        {
            var box = Factory.CreateBox("Box", 10, 10, 10, 0, 10, 10, 10, 100);
            var itemA = Factory.CreateItem("Item A", 5, 10, 10, 10, true);
            var itemB = Factory.CreateItem("Item B", 5, 10, 10, 20, true);

            var packedItemA = new PackedItem(itemA, 0, 0, 0, 5, 10, 10);
            var packedItemB = new PackedItem(itemB, 0, 0, 0, 5, 10, 10);

            var packedItemListA = new PackedItemList();
            packedItemListA.Insert(packedItemA);
            var packedBoxA = new PackedBox(box, packedItemListA);

            var packedItemListB = new PackedItemList();
            packedItemListB.Insert(packedItemB);
            var packedBoxB = new PackedBox(box, packedItemListB);

            var pBoxArray = new PackedBox[] { packedBoxA, packedBoxB };

            var packedBoxList = new PackedBoxList();
            packedBoxList.Insert(packedBoxA);
            packedBoxList.Insert(packedBoxB);

            Assert.Equal(packedBoxA, packedBoxList.Top());
        }

        [Fact]
        public void TestVolumeUtilisation()
        {
            var box = Factory.CreateBox("Box", 10, 10, 10, 0, 10, 10, 10, 10);
            var item = Factory.CreateItem("Item", 5, 10, 10, 10, true);

            var packedItem = new PackedItem(item, 0, 0, 0, 5, 10, 10);

            var packedItemList = new PackedItemList();
            packedItemList.Insert(packedItem);
            var packedBox = new PackedBox(box, packedItemList);

            var packedBoxList = new PackedBoxList();
            packedBoxList.Insert(packedBox);

            Assert.Equal(50f, packedBoxList.GetVolumeUtilisation());
        }

        [Fact]
        public void TestWeightVariance()
        {
            var box = Factory.CreateBox("Box", 10, 10, 10, 0, 10, 10, 10, 100);
            var itemA = Factory.CreateItem("Item A", 5, 10, 10, 10, true);
            var itemB = Factory.CreateItem("Item B", 5, 10, 10, 20, true);

            var packedItemA = new PackedItem(itemA, 0, 0, 0, 5, 10, 10);
            var packedItemB = new PackedItem(itemB, 0, 0, 0, 5, 10, 10);

            var packedItemListA = new PackedItemList();
            packedItemListA.Insert(packedItemA);
            var packedBoxA = new PackedBox(box, packedItemListA);

            var packedItemListB = new PackedItemList();
            packedItemListB.Insert(packedItemB);
            var packedBoxB = new PackedBox(box, packedItemListB);

            var pBoxArray = new PackedBox[] { packedBoxA, packedBoxB };

            var packedBoxList = new PackedBoxList();
            packedBoxList.Insert(packedBoxA);
            packedBoxList.Insert(packedBoxB);

            Assert.Equal(25, packedBoxList.GetWeightVariance());
        }

        [Fact]
        public void TestMeanWeight()
        {
            var box = Factory.CreateBox("Box", 10, 10, 10, 0, 10, 10, 10, 100);
            var itemA = Factory.CreateItem("Item A", 5, 10, 10, 10, true);
            var itemB = Factory.CreateItem("Item B", 5, 10, 10, 20, true);

            var packedItemA = new PackedItem(itemA, 0, 0, 0, 5, 10, 10);
            var packedItemB = new PackedItem(itemB, 0, 0, 0, 5, 10, 10);

            var packedItemListA = new PackedItemList();
            packedItemListA.Insert(packedItemA);
            var packedBoxA = new PackedBox(box, packedItemListA);

            var packedItemListB = new PackedItemList();
            packedItemListB.Insert(packedItemB);
            var packedBoxB = new PackedBox(box, packedItemListB);

            var pBoxArray = new PackedBox[] { packedBoxA, packedBoxB };

            var packedBoxList = new PackedBoxList();
            packedBoxList.Insert(packedBoxA);
            packedBoxList.Insert(packedBoxB);

            Assert.Equal(15, packedBoxList.GetMeanWeight());
        }

    }
}
