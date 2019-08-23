using SharpPacker.Models;
using SharpPacker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SharpPacker.Tests.BoxPackerTests
{
    public class VolumePackerTest
    {
        /// <summary>
        /// Test that identical orientation doesn't survive change of row
        /// (7 side by side, then 2 side by side rotated).
        /// </summary>
        [Fact]
        public void TestAllowsRotatedBoxesInNewRow()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test an infinite loop doesn't come back.
        /// </summary>
        [Fact]
        public void TestIssue14()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIssue147A()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIssue147B()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIssue148()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIssue161()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIssue164()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestIssue174()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test identical items keep their orientation (with box length > width).
        /// </summary>
        [Fact]
        public void TestIssue47A()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test identical items keep their orientation (with box length < width).
        /// </summary>
        [Fact]
        public void TestIssue47B()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestUnpackedSpaceInsideLayersIsFilled()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// From issue #79.
        /// </summary>
        [Fact]
        public void TestUsedDimensionsCalculatedCorrectly()
        {
            var box = Factory.CreateBox("Bundle", 75, 15, 15, 0, 75, 15, 15, 30);
            var itemList = new List<Item4d>
            {
                Factory.CreateItem("Item 1", 14, 12, 2, 2, true),
                Factory.CreateItem("Item 2", 14, 12, 2, 2, true),
                Factory.CreateItem("Item 3", 14, 12, 2, 2, true),
                Factory.CreateItem("Item 4", 14, 12, 2, 2, true),
                Factory.CreateItem("Item 5", 14, 12, 2, 2, true),
            };

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

            #pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
            Assert.Equal(1, pBoxes.Count);

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