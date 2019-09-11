using BoxPackerClone.Models;
using Xunit;

namespace BoxPackerClone.Tests
{
    public class OrientatedItemTest
    {
        /// <summary>
        /// Sometimes people use a 0 depth.
        /// </summary>
        [Fact]
        public void TestZeroDepth()
        {
            var orientedItem = new OrientatedItem(
                    Factory.CreateItem("Item", 1, 1, 0, 0, false),
                    1,
                    1,
                    0
                );
            var tippingPoint = orientedItem.GetTippingPoint();
        }
    }
}
