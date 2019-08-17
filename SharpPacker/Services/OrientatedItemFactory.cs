using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Services
{
    internal class OrientatedItemFactory
    {
        private readonly Box4d box;

        public OrientatedItemFactory(Box4d _box)
        {
            box = _box;
        }

        protected int CalculateAdditionalItemsPackedWithThisOrientation(OrientatedItem4d prevItem,
                                                                        IEnumerable<Item4d> nextItems,
                                                                        int originalWidthLeft,
                                                                        int originalLengthLeft,
                                                                        int depthLeft,
                                                                        int currentRowLengthBeforePacking)
        {
            var packedCount = 0;
            var currentRowLength = Math.Max(prevItem.Length, currentRowLengthBeforePacking);

            // cap lookahead as this gets recursive and slow
            var capThreshold = 8;
            var itemsToPack = nextItems.OrderByDescending(i => i).Take(capThreshold).Reverse();

            var tempBox = new WorkingVolume4d(originalWidthLeft - prevItem.Width, currentRowLength, depthLeft, int.MaxValue);
            var tempPacker = new VolumePacker(tempBox, itemsToPack.ToList());

            tempPacker.LookAheadMode = true;

            var remainigRowPacked = tempPacker.Pack();

            itemsToPack = itemsToPack.Except(remainigRowPacked.Items.Select(x => x.Item));

            return nextItems.Count() - itemsToPack.Count();
        }
    }
}

l