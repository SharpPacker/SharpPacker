using BoxPackerClone.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BoxPackerClone.Tests.BoxPackerTests
{
    public class PackerTest
    {
        [Fact]
        public void TestPackThreeItemsOneDoesntFitInAnyBox()
        {
            var box1 = Factory.CreateBox("Le petite box", 300, 300, 10, 10, 296, 296, 8, 1000);
            var box2 = Factory.CreateBox("Le grande box", 3000, 3000, 100, 100, 2960, 2960, 80, 10000);

            var item1 = Factory.CreateItem("Item 1", 2500, 2500, 20, 2000, true);
            var item2 = Factory.CreateItem("Item 2", 25000, 2500, 20, 2000, true);
            var item3 = Factory.CreateItem("Item 3", 2500, 2500, 20, 2000, true);

            var packer = new Packer();
            packer.AddBox(box1);
            packer.AddBox(box2);
            packer.AddItem(item1);
            packer.AddItem(item2);
            packer.AddItem(item3);

            Exception exception = null;

            try
            {
                packer.Pack();
            }
            catch(Exception e)
            {
                exception = e;
            }

            Assert.IsType<ItemTooLargeException>(exception);
        }

        [Fact]
        public void TestPackWithoutBox()
        {
            var item1 = Factory.CreateItem("Item 1", 2500, 2500, 20, 2000, true);
            var item2 = Factory.CreateItem("Item 2", 25000, 2500, 20, 2000, true);
            var item3 = Factory.CreateItem("Item 3", 2500, 2500, 20, 2000, true);

            var packer = new Packer();
            packer.AddItem(item1);
            packer.AddItem(item2);
            packer.AddItem(item3);

            Exception exception = null;

            try
            {
                packer.Pack();
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.IsType<ItemTooLargeException>(exception);
        }

        /// <summary>
        /// Test weight distribution getter/setter.
        /// </summary>
        [Fact]
        public void TestCanSetMaxBoxesToWeightBalance()
        {
            var packer = new Packer();
            packer.MaxBoxesToBalanceWeight = 3;

            Assert.Equal(3, packer.MaxBoxesToBalanceWeight);
        }

        /// <summary>
        /// Test that weight redistribution activates (or not) correctly based on the current limit.
        /// </summary>
        [Fact]
        public void TestWeightRedistributionActivatesOrNot()
        {
            // first pack normally - expecting 2+2 after balancing
            var packer1 = new Packer();
            packer1.AddBox(Factory.CreateBox("Box", 1, 1, 3, 0, 1, 1, 3, 3));
            packer1.AddItem(Factory.CreateItem("Item", 1, 1, 1, 1, false), 4);

            var packedBoxes1 = packer1.Pack().ToList();

            Assert.Equal(2, packedBoxes1[0].PackedItems.Count);
            Assert.Equal(2, packedBoxes1[1].PackedItems.Count);

            // same items, but with redistribution turned off - expecting 3+1 based on pure fit
            var packer2 = new Packer();
            packer2.AddBox(Factory.CreateBox("Box", 1, 1, 3, 0, 1, 1, 3, 3));
            packer2.AddItem(Factory.CreateItem("Item", 1, 1, 1, 1, false), 4);
            packer2.MaxBoxesToBalanceWeight = 1;

            var packedBoxes2 = packer2.Pack().ToList();

            Assert.Equal(3, packedBoxes2[0].PackedItems.Count);
            Assert.Single(packedBoxes2[1].PackedItems);
        }

        /// <summary>
        /// Test used width calculations on a case where it used to fail.
        /// </summary>
        [Fact]
        public void TestIssue52A()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box", 100, 50, 50, 0, 100, 50, 50, 5000));
            packer.AddItem(Factory.CreateItem("Item", 15, 13, 8, 407, true), 2);
            var packedBoxes = packer.Pack().ToList();

            Assert.Single(packedBoxes);
            Assert.Equal(26, packedBoxes[0].UsedWidth);
            Assert.Equal(15, packedBoxes[0].UsedLength);
            Assert.Equal(8, packedBoxes[0].UsedDepth);
        }

        /// <summary>
        /// Test used width calculations on a case where it used to fail.
        /// </summary>
        [Fact]
        public void TestIssue52B()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box", 370, 375, 60, 140, 364, 374, 40, 3000));
            packer.AddItem(Factory.CreateItem("Item 1", 220, 310, 12, 679, true));
            packer.AddItem(Factory.CreateItem("Item 2", 210, 297, 11, 648, true));
            packer.AddItem(Factory.CreateItem("Item 3", 210, 297, 5, 187, true));
            packer.AddItem(Factory.CreateItem("Item 4", 148, 210, 32, 880, true));
            var packedBoxes = packer.Pack().ToList();

            Assert.Single(packedBoxes);
            Assert.Equal(310, packedBoxes[0].UsedWidth);
            Assert.Equal(368, packedBoxes[0].UsedLength);
            Assert.Equal(32, packedBoxes[0].UsedDepth);
        }

        /// <summary>
        /// Test used width calculations on a case where it used to fail.
        /// </summary>
        [Fact]
        public void TestIssue52C()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box", 230, 300, 240, 160, 230, 300, 240, 15000));
            packer.AddItem(Factory.CreateItem("Item 1", 210, 297, 4, 213, true));
            packer.AddItem(Factory.CreateItem("Item 2", 80, 285, 70, 199, true));
            packer.AddItem(Factory.CreateItem("Item 3", 80, 285, 70, 199, true));
            var packedBoxes = packer.Pack().ToList();

            Assert.Single(packedBoxes);
            Assert.Equal(210, packedBoxes[0].UsedWidth);
            Assert.Equal(297, packedBoxes[0].UsedLength);
            Assert.Equal(74, packedBoxes[0].UsedDepth);
        }

        /// <summary>
        /// Test case where last item algorithm picks a slightly inefficient box.
        /// </summary>
        [Fact]
        public void TestIssue117()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box A", 36, 8, 3, 0, 36, 8, 3, 2));
            packer.AddBox(Factory.CreateBox("Box B", 36, 8, 8, 0, 36, 8, 8, 2));
            packer.AddItem(Factory.CreateItem("Item 1", 35, 7, 2, 1, false));
            packer.AddItem(Factory.CreateItem("Item 2", 6, 5, 1, 1, false));

            var packedBoxes = packer.Pack().ToList();

            Assert.Single(packedBoxes);
            Assert.Equal("Box A", packedBoxes[0].Box.Reference);
        }

        /// <summary>
        /// Where 2 perfectly filled boxes are a choice, need to ensure we pick the larger one or there is a cascading
        /// failure of many small boxes instead of a few larger ones.
        /// </summary>
        [Fact]
        public void TestIssue38()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box1", 2, 2, 2, 0, 2, 2, 2, 1000));
            packer.AddBox(Factory.CreateBox("Box2", 4, 4, 4, 0, 4, 4, 4, 1000));

            packer.AddItem(Factory.CreateItem("Item 1", 1, 1, 1, 100, false));
            packer.AddItem(Factory.CreateItem("Item 2", 1, 1, 1, 100, false));
            packer.AddItem(Factory.CreateItem("Item 3", 1, 1, 1, 100, false));
            packer.AddItem(Factory.CreateItem("Item 4", 1, 1, 1, 100, false));
            packer.AddItem(Factory.CreateItem("Item 5", 2, 2, 2, 100, false));
            packer.AddItem(Factory.CreateItem("Item 6", 2, 2, 2, 100, false));
            packer.AddItem(Factory.CreateItem("Item 7", 2, 2, 2, 100, false));
            packer.AddItem(Factory.CreateItem("Item 8", 2, 2, 2, 100, false));
            packer.AddItem(Factory.CreateItem("Item 9", 4, 4, 4, 100, false));

            var packedBoxes = packer.Pack();
            Assert.Equal(2, packedBoxes.Count);
        }

        /// <summary>
        /// From issue #168.
        /// </summary>
        [Fact]
        public void TestIssue168()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Small", 85, 190, 230, 30, 85, 190, 230, 10000));
            packer.AddBox(Factory.CreateBox("Medium", 220, 160, 160, 50, 220, 160, 160, 10000));
            packer.AddItem(Factory.CreateItem("Item", 55, 85, 122, 350, false));

            var packedBoxes = packer.Pack().ToList();

            Assert.Single(packedBoxes);
            Assert.Equal("Small", packedBoxes[0].Box.Reference);
        }

        /// <summary>
        /// From issue #170.
        /// </summary>
        [Fact]
        public void TestIssue170()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box", 170, 120, 120, 2000, 170, 120, 120, 60000));
            packer.AddItem(Factory.CreateItem("Item", 70, 130, 2, 657, false), 100);

            var packedBox = packer.Pack();

            Assert.Equal(2, packedBox.Count);
        }
    }
}
