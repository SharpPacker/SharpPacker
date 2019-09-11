using Xunit;

namespace BoxPackerClone.Tests.BoxPackerTests
{
    public class WeightRedistributorTest
    {
        /// <summary>
        /// Test that a native 3+1 is repacked into 2+2.
        /// </summary>
        [Fact]
        public void TestWeightRedistributionActivatesOrNot()
        {
            var packer = new Packer();
            packer.AddBox(Factory.CreateBox("Box", 1, 1, 3, 0, 1, 1, 3, 3));
            packer.AddItem(Factory.CreateItem("Item", 1, 1, 1, 1, false), 4);

            var pBoxes = packer.Pack().ToList();

            Assert.Equal(2, pBoxes[0].PackedItems.Count);
            Assert.Equal(2, pBoxes[1].PackedItems.Count);
        }
    }
}
