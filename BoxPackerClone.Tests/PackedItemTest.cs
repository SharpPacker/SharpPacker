﻿using BoxPackerClone.Models;
using Xunit;

namespace BoxPackerClone.Tests
{
    public class PackedItemTest
    {
        /// <summary>
        /// Test volume calculation
        /// </summary>
        [Fact]
        public void TestVolumeCalculation()
        {
            var pItem = new PackedItem(
                    Factory.CreateItem("Item", 1, 1, 0, 0, true),
                    0,
                    0,
                    0,
                    3,
                    5,
                    7
                );

            Assert.Equal(105, pItem.Volume);
        }
    }
}