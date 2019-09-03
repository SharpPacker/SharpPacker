using SharpPacker.Models;
using System.Collections.Generic;
using Xunit;

namespace SharpPacker.Tests.BoxPackerTests
{
    public class PackedBoxTest
    {
        /// <summary>
        /// Test various getters work correctly.
        /// </summary>
        [Fact]
        public void TestGetters()
        {
            var box = Factory.CreateBox("Box", 370, 375, 60, 140, 364, 374, 40, 3000);
            var oItem = new OrientatedItem(
                    Factory.CreateItem("Item", 230, 330, 6, 320, true),
                    230,
                    330,
                    6
                );

            var packedItemList = new PackedItemList();
            packedItemList.Insert(PackedItem.FromOrientatedItem(oItem, 0, 0, 0));

            var pBox = new PackedBox(box, packedItemList);

            Assert.Equal(box.Reference, pBox.Box.Reference);
            Assert.Equal(460, pBox.TotalWeight);

            Assert.Equal(134, pBox.RemainingWidth);
            Assert.Equal(44, pBox.RemainingLength);
            Assert.Equal(34, pBox.RemainingDepth);

            Assert.Equal(2540, pBox.RemainingWeight);

            Assert.Equal(5445440, pBox.InnerVolume);
        }

        /// <summary>
        /// Test that volume utilisation is calculated correctly.
        /// </summary>
        [Fact]
        public void TestVolumeUtilisation()
        {
            var box = Factory.CreateBox("Box", 10, 10, 20, 10, 10, 10, 20, 10);
            var oItem = new OrientatedItem(
                    Factory.CreateItem("Item", 4, 10, 10, 10, true),
                    4,
                    10,
                    10
                );

            var packedItemList = new PackedItemList();
            packedItemList.Insert(PackedItem.FromOrientatedItem(oItem, 0, 0, 0));

            var pBox = new PackedBox(box, packedItemList);

            Assert.Equal(400, pBox.UsedVolume);
            Assert.Equal(1600, pBox.UnusedVolume);
            Assert.Equal(20, pBox.VolumeUtilizationPercent);
        }
    }
}
