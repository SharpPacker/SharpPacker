using BoxPackerClone.Models;
using BoxPackerClone.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BoxPackerClone.Tests
{
    public class VolumePackerTest
    {
        /// <summary>
        /// Test that identical orientation doesn"t survive change of row (7 side by side, then 2
        /// side by side rotated).
        /// </summary>
        [Fact]
        public void TestAllowsRotatedBoxesInNewRow()
        {
            var box = Factory.CreateBox("40x70x30InternalBox", 40, 70, 30, 0, 40, 70, 30, 1000);
            var item = Factory.CreateItem("30x10x30item", 30, 10, 30, 0, true);

            var itemList = new ItemList();
            for(var i = 0; i < 9; i++)
            {
                itemList.Insert(item);
            }

            var packer = new VolumePacker(box, itemList);
            var packedBox = packer.Pack();

            Assert.Equal(9, packedBox.PackedItems.Count);
        }

        /// <summary>
        /// Test an infinite loop doesn"t come back.
        /// </summary>
        [Fact]
        public void TestIssue14()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("29x1x23Box", 29, 1, 23, 0, 29, 1, 23, 100));
            packer.AddItem(Factory.CreateItem("13x1x10Item", 13, 1, 10, 1, true));
            packer.AddItem(Factory.CreateItem("9x1x6Item", 9, 1, 6, 1, true));
            packer.AddItem(Factory.CreateItem("9x1x6Item", 9, 1, 6, 1, true));
            packer.AddItem(Factory.CreateItem("9x1x6Item", 9, 1, 6, 1, true));

            var pBoxes = packer.Pack();
            var boxesCount = pBoxes.Count;

            Assert.Equal(1, boxesCount);
        }

        /// <summary>
        /// From issue #147.
        /// </summary>
        [Fact]
        public void TestIssue147A()
        {
            var box = Factory.CreateBox("Box", 250, 1360, 260, 0, 250, 1360, 260, 30000);
            var item = Factory.CreateItem("Item", 90, 200, 200, 150, true);

            var itemList = new ItemList();

            for(var i = 0; i < 14; i++)
            {
                itemList.Insert(item);
            }

            var packer = new VolumePacker(box, itemList);
            var packedBox = packer.Pack();

            Assert.Equal(14, packedBox.PackedItems.Count);
        }

        /// <summary>
        /// From issue #147.
        /// </summary>
        [Fact]
        public void TestIssue147B()
        {
            var box1 = Factory.CreateBox("Box", 400, 200, 500, 0, 400, 200, 500, 10000);
            var itemList1 = new ItemList();
            itemList1.Insert(Factory.CreateItem("Item 1", 447, 62, 303, 965, false));
            itemList1.Insert(Factory.CreateItem("Item 2", 495, 70, 308, 1018, false));

            var packer1 = new VolumePacker(box1, itemList1);
            var packedBox1 = packer1.Pack();

            var box2 = Factory.CreateBox("Box", 400, 200, 500, 0, 400, 200, 500, 10000);
            var itemList2 = new ItemList();
            itemList2.Insert(Factory.CreateItem("Item 1", 447, 62, 303, 965, false));
            itemList2.Insert(Factory.CreateItem("Item 2", 495, 70, 308, 1018, false));

            var packer2 = new VolumePacker(box1, itemList2);
            var packedBox2= packer2.Pack();

            Assert.Equal(2, packedBox1.PackedItems.Count);
            Assert.Equal(2, packedBox2.PackedItems.Count);
        }

        /// <summary>
        /// Test stability of items is calculated appropriately.
        /// </summary>
        [Fact]
        public void TestIssue148()
        {
            var box = Factory.CreateBox("Box", 27, 37, 22, 100, 25, 36, 21, 15000);
            var item = Factory.CreateItem("Item", 6, 12, 20, 100, false);
            var itemList = new ItemList();
            for(var i = 0; i < 12; i++)
            {
                itemList.Insert(item);
            }

            var packer = new VolumePacker(box, itemList);
            var packedBox = packer.Pack();

            Assert.Equal(12, packedBox.PackedItems.Count);

            var box2 = Factory.CreateBox("Box", 27, 37, 22, 100, 25, 36, 21, 15000);
            var item2 = Factory.CreateItem("Item", 6, 12, 20, 100, true);
            var itemList2 = new ItemList();
            for (var i = 0; i < 12; i++)
            {
                itemList2.Insert(item2);
            }

            var packer2 = new VolumePacker(box2, itemList2);
            var packedBox2 = packer2.Pack();

            Assert.Equal(12, packedBox2.PackedItems.Count);
        }

        /// <summary>
        /// From issue #161.
        /// </summary>
        [Fact]
        public void TestIssue161()
        {
            var box = Factory.CreateBox("Box", 240, 150, 180, 0, 240, 150, 180, 10000);
            var item1 = Factory.CreateItem("Item 1", 70, 70, 95, 0, false);
            var item2 = Factory.CreateItem("Item 2", 95, 75, 95, 0, true);

            var itemList1 = new ItemList();
            for(var i = 0; i < 6; i++)
            {
                itemList1.Insert(item1);
            }
            for (var i = 0; i < 3; i++)
            {
                itemList1.Insert(item2);
            }

            var packer1 = new VolumePacker(box, itemList1);
            var packedBox1 = packer1.Pack();
            Assert.Equal(9, packedBox1.PackedItems.Count);

            var itemList2 = new ItemList();
            for (var i = 0; i < 6; i++)
            {
                itemList2.Insert(item1);
            }
            for (var i = 0; i < 2; i++)
            {
                itemList2.Insert(item2);
            }

            var packer2 = new VolumePacker(box, itemList2);
            var packedBox = packer2.Pack();
            Assert.Equal(8, packedBox.PackedItems.Count);
        }

        /// <summary>
        /// From issue #164.
        /// </summary>
        [Fact]
        public void TestIssue164()
        {
            var box = Factory.CreateBox("Box", 820, 820, 830, 0, 820, 820, 830, 10000);
            var itemList = new ItemList();
            itemList.Insert(Factory.CreateItem("Item 1", 110, 110, 50, 100, false));
            itemList.Insert(Factory.CreateItem("Item 2", 100, 300, 30, 100, false));
            itemList.Insert(Factory.CreateItem("Item 3", 100, 150, 50, 100, false));
            itemList.Insert(Factory.CreateItem("Item 4", 100, 200, 80, 110, false));
            itemList.Insert(Factory.CreateItem("Item 5", 80, 150, 80, 50, false));
            itemList.Insert(Factory.CreateItem("Item 6", 80, 150, 80, 50, false));
            itemList.Insert(Factory.CreateItem("Item 7", 80, 150, 80, 50, false));
            itemList.Insert(Factory.CreateItem("Item 8", 270, 70, 60, 350, false));
            itemList.Insert(Factory.CreateItem("Item 9", 150, 150, 80, 180, false));
            itemList.Insert(Factory.CreateItem("Item 10", 80, 150, 80, 50, false));

            var packer = new VolumePacker(box, itemList);
            var packedBox = packer.Pack();

            Assert.Equal(10, packedBox.PackedItems.Count);
        }

        [Fact]
        public void TestIssue174()
        {
            var box = Factory.CreateBox("Box", 0, 0, 0, 10, 5000, 5000, 5000, 10000);

            var itemList = new ItemList();
            itemList.Insert(Factory.CreateItem("Item 0", 1000, 1650, 850, 500, false));
            itemList.Insert(Factory.CreateItem("Item 1", 960, 1640, 800, 500, false));
            itemList.Insert(Factory.CreateItem("Item 2", 950, 1650, 800, 500, false));
            itemList.Insert(Factory.CreateItem("Item 3", 1000, 2050, 800, 500, false));
            itemList.Insert(Factory.CreateItem("Item 4", 1000, 2100, 850, 500, false));
            itemList.Insert(Factory.CreateItem("Item 5", 950, 2050, 800, 500, false));
            itemList.Insert(Factory.CreateItem("Item 6", 940, 970, 800, 500, false));

            var volumePacker = new VolumePacker(box, itemList);
            var packedBox = volumePacker.Pack();

            Assert.Equal(7, packedBox.PackedItems.Count);
        }

        /// <summary>
        /// Test identical items keep their orientation (with box length > width).
        /// </summary>
        [Fact]
        public void TestIssue47A()
        {
            var box = Factory.CreateBox("165x225x25Box", 165, 225, 25, 0, 165, 225, 25, 100);
            var item = Factory.CreateItem("20x69x20Item", 20, 69, 20, 0, true);

            var itemList = new ItemList();
            for(var i = 0; i < 23; i++)
            {
                itemList.Insert(item);
            }

            var packer = new VolumePacker(box, itemList);
            var packedBox = packer.Pack();

            Assert.Equal(23, packedBox.PackedItems.Count);
        }

        /// <summary>
        /// Test identical items keep their orientation (with box length < width)
        /// </summary>
        [Fact]
        public void TestIssue47B()
        {
            var box = Factory.CreateBox("165x225x25Box", 165, 225, 25, 0, 165, 225, 25, 100);
            var item = Factory.CreateItem("20x69x20Item", 69, 20, 20, 0, true);

            var itemList = new ItemList();
            for(var i = 0; i < 23; i++)
            {
                itemList.Insert(item);
            }

            var packer = new VolumePacker(box, itemList);
            var packedBox = packer.Pack();

            Assert.Equal(23, packedBox.PackedItems.Count);
        }

        /// <summary>
        /// From issue #124.
        /// </summary>
        [Fact(Skip = "until bug is fixed")]
        public void TestUnpackedSpaceInsideLayersIsFilled()
        {
            var box1 = Factory.CreateBox("Box", 4, 14, 11, 0, 4, 14, 11, 100);
            var itemList1 = new ItemList();
            itemList1.Insert(Factory.CreateItem("Item 1", 8, 8, 2, 1, false));
            itemList1.Insert(Factory.CreateItem("Item 1", 4, 4, 4, 1, false));
            itemList1.Insert(Factory.CreateItem("Item 1", 4, 4, 4, 1, false));

            var packer1 = new VolumePacker(box1, itemList1);
            var packedBox1 = packer1.Pack();

            Assert.Equal(3, packedBox1.PackedItems.Count);

            var box2 = Factory.CreateBox("Box", 4, 14, 11, 0, 4, 14, 11, 100);
            var itemList2 = new ItemList();
            itemList1.Insert(Factory.CreateItem("Item 1", 8, 8, 2, 1, false));
            itemList1.Insert(Factory.CreateItem("Item 1", 4, 4, 4, 1, false));
            itemList1.Insert(Factory.CreateItem("Item 1", 4, 4, 4, 1, false));

            var packer2 = new VolumePacker(box2, itemList2);
            var packedBox2 = packer2.Pack();

            Assert.Equal(3, packedBox2.PackedItems.Count);
        }

        /// <summary>
        /// From issue #79.
        /// </summary>
        [Fact]
        public void TestUsedDimensionsCalculatedCorrectly()
        {
            var box = Factory.CreateBox("Bundle", 75, 15, 15, 0, 75, 15, 15, 30);
            var itemList = new ItemList();
            itemList.Insert(Factory.CreateItem("Item 1", 14, 12, 2, 2, true));
            itemList.Insert(Factory.CreateItem("Item 2", 14, 12, 2, 2, true));
            itemList.Insert(Factory.CreateItem("Item 3", 14, 12, 2, 2, true));
            itemList.Insert(Factory.CreateItem("Item 4", 14, 12, 2, 2, true));
            itemList.Insert(Factory.CreateItem("Item 5", 14, 12, 2, 2, true));

            var packer = new VolumePacker(box, itemList);
            var pBox = packer.Pack();

            Assert.Equal(60, pBox.UsedWidth);
            Assert.Equal(14, pBox.UsedLength);
            Assert.Equal(2, pBox.UsedDepth);
        }

        /// <summary>
        /// From issue #86.
        /// </summary>
        [Fact]
        public void TestUsedWidthAndRemainingWidthHandleRotationsCorrectly()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box", 23, 27, 14, 0, 23, 27, 14, 30));
            packer.AddItem(Factory.CreateItem("Item 1", 11, 22, 2, 1, true), 3);
            packer.AddItem(Factory.CreateItem("Item 2", 11, 22, 2, 1, true), 4);
            packer.AddItem(Factory.CreateItem("Item 3", 6, 17, 2, 1, true), 3);

            var pBoxes = packer.Pack();

            var boxesCount = pBoxes.Count;
            Assert.Equal(1, boxesCount);

            var pBox = pBoxes.First();

            Assert.Equal(22, pBox.UsedWidth);
            Assert.Equal(23, pBox.UsedLength);
            Assert.Equal(10, pBox.UsedDepth);
            Assert.Equal(1, pBox.RemainingWidth);
            Assert.Equal(4, pBox.RemainingLength);
            Assert.Equal(4, pBox.RemainingDepth);
        }
    }
}
