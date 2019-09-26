using SharpPacker.Strategy.BoxPackerClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Strategy.BoxPackerClone.Services
{
    internal class OrientatedItemFactory
    {
        private readonly Box box;

        public OrientatedItemFactory(Box _box)
        {
            box = _box;
        }

        public OrientatedItem GetBestOrientation(
                               Item item,
                               OrientatedItem prevItem,
                               ItemList nextItems,
                               bool isLastItem,
                               int widthLeft,
                               int lengthLeft,
                               int depthLeft,
                               int rowLength,
                               int x,
                               int y,
                               int z,
                               PackedItemList prevPackedItemList
                           )
        {
            var possibleOrientations = GetPossibleOrientations(item, prevItem, widthLeft, lengthLeft, depthLeft, x, y, z, prevPackedItemList);
            var usableOrientations = GetUsableOrientations(item, possibleOrientations, isLastItem);

            if (usableOrientations.Count == 0)
            {
                return null;
            }
            //TODO: VolumePackerTest.161 chtck here
            var comparer = new OrientatedItemsComparer(this)
            {
                widthLeft = widthLeft,
                lengthLeft = lengthLeft,
                depthLeft = depthLeft,
                nextItems = nextItems,
                rowLength = rowLength,
                x = x,
                y = y,
                z = z,
                prevPackedItemList = prevPackedItemList,
            };
            usableOrientations.Sort(comparer);

            var bestFit = usableOrientations.First();

            return bestFit;
        }

        public List<OrientatedItem> GetPossibleOrientations(Item item,
            OrientatedItem prevItem,
            int widthLeft,
            int lengthLeft,
            int depthLeft,
            int x,
            int y,
            int z,
            PackedItemList prevPackedItemsList)
        {
            var orientations = new List<OrientatedItem>();

            if (prevItem != null && IsSameDimensions(prevItem.Item, item))
            {
                // Special case items that are the same as what we just packed - keep orientation
                orientations.Add(
                    new OrientatedItem()
                    {
                        Item = item,
                        Width = prevItem.Width,
                        Length = prevItem.Length,
                        Depth = prevItem.Depth
                    }
                );
            }
            else
            {
                // simple 2D rotation
                orientations.Add(
                    new OrientatedItem()
                    {
                        Item = item,
                        Width = item.Width,
                        Length = item.Length,
                        Depth = item.Depth
                    }
                );
                orientations.Add(
                    new OrientatedItem()
                    {
                        Item = item,
                        Width = item.Length,
                        Length = item.Width,
                        Depth = item.Depth
                    }
                );

                //add 3D rotation if we're allowed
                if (!item.KeepFlat)
                {
                    orientations.Add(
                        new OrientatedItem()
                        {
                            Item = item,
                            Width = item.Width,
                            Length = item.Depth,
                            Depth = item.Length
                        }
                    );

                    orientations.Add(
                        new OrientatedItem()
                        {
                            Item = item,
                            Width = item.Length,
                            Length = item.Depth,
                            Depth = item.Width
                        }
                    );

                    orientations.Add(
                        new OrientatedItem()
                        {
                            Item = item,
                            Width = item.Depth,
                            Length = item.Width,
                            Depth = item.Length
                        }
                    );

                    orientations.Add(
                        new OrientatedItem()
                        {
                            Item = item,
                            Width = item.Depth,
                            Length = item.Length,
                            Depth = item.Width
                        }
                    );
                }
            }

            // remove any that simply don't fit
            orientations = orientations.
                Distinct().
                Where(or => or.Width <= widthLeft && or.Length <= lengthLeft && or.Depth <= depthLeft).
                ToList();

            // TODO: implement ConstrainedPlacementItem case

            return orientations;
        }

        public List<OrientatedItem> GetPossibleOrientationsInEmptyBox(Item item)
        {
            // TODO: cache implementation
            var orientations = GetPossibleOrientations(item,
                                                        null,
                                                        box.InnerWidth,
                                                        box.InnerLength,
                                                        box.InnerDepth,
                                                        0,
                                                        0,
                                                        0,
                                                        new PackedItemList()
                                                    );

            return orientations;
        }

        public List<OrientatedItem> GetUsableOrientations(Item item, IEnumerable<OrientatedItem> possibleOrientations, bool isLastItem)
        {
            var orientationsToUse = new List<OrientatedItem>();
            var stableOrientations = new List<OrientatedItem>();
            var unstableOrientations = new List<OrientatedItem>();

            // Divide possible orientations into stable (low centre of gravity) and unstable (high
            // centre of gravity)
            foreach (var orientation in possibleOrientations)
            {
                if (orientation.IsStable())
                {
                    stableOrientations.Add(orientation);
                }
                else
                {
                    unstableOrientations.Add(orientation);
                }
            }

            // We prefer to use stable orientations only, but allow unstable ones if either the item
            // is the last one left to pack OR the item doesn't fit in the box any other way
            if (stableOrientations.Count > 0)
            {
                orientationsToUse = stableOrientations;
            }
            else if (unstableOrientations.Count > 0)
            {
                if (isLastItem)
                {
                    orientationsToUse = unstableOrientations;
                }
                else
                {
                    var stableOrientationsInEmptyBox = GetStableOrientationsInEmptyBox(item);
                    if (stableOrientationsInEmptyBox.Count() == 0)
                    {
                        orientationsToUse = unstableOrientations;
                    }
                }
            }

            return orientationsToUse;
        }

        protected int CalculateAdditionalItemsPackedWithThisOrientation(OrientatedItem prevItem,
                                                                        ItemList nextItems,
                                                                        int originalWidthLeft,
                                                                        int originalLengthLeft,
                                                                        int depthLeft,
                                                                        int currentRowLengthBeforePacking)
        {
            var currentRowLength = Math.Max(prevItem.Length, currentRowLengthBeforePacking);

            // cap lookahead as this gets recursive and slow
            var itemsToPack = nextItems.TopN(8);

            var tempBox = new WorkingVolume(originalWidthLeft - prevItem.Width, currentRowLength, depthLeft, int.MaxValue);
            var tempPacker = new VolumePacker(tempBox, itemsToPack.Clone());
            tempPacker.LookAheadMode = true;

            var remainigRowPacked = tempPacker.Pack();
            foreach(var packedItem in remainigRowPacked.PackedItems)
            {
                itemsToPack.Remove(packedItem.Item);
            }

            var tempBox2 = new WorkingVolume(originalWidthLeft, originalLengthLeft - currentRowLength, depthLeft, int.MaxValue);
            var tempPacker2 = new VolumePacker(tempBox2, itemsToPack.Clone());
            tempPacker2.LookAheadMode = true;
            var nextRowPacked = tempPacker2.Pack();
            foreach (var packedItem in nextRowPacked.PackedItems)
            {
                itemsToPack.Remove(packedItem.Item);
            }

            return nextItems.Count() - itemsToPack.Count();
        }

        /// <summary>
        /// Return the orientations for this item if it were to be placed into the box with nothing else.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<OrientatedItem> GetStableOrientationsInEmptyBox(Item item)
        {
            var orientations = GetPossibleOrientationsInEmptyBox(item);
            return orientations.Where(o => o.IsStable()).ToList();
        }

        private List<OrientatedItem> GetStablePrientationsInEmptyBox(Item item)
        {
            var orientationsInEmptyBox = GetPossibleOrientationsInEmptyBox(item);

            return orientationsInEmptyBox.Where(o => o.IsStable()).ToList();
        }

        /// <summary>
        /// Compare two items to see if they have same dimensions.
        /// </summary>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        /// <returns></returns>
        public bool IsSameDimensions(Item itemA, Item itemB)
        {
            var dimsA = new int[] { itemA.Width, itemA.Length, itemA.Depth };
            var dimsB = new int[] { itemB.Width, itemB.Length, itemB.Depth };

            dimsA.OrderBy(d => d);
            dimsB.OrderBy(d => d);

            return dimsA[0] == dimsB[0]
                    && dimsA[1] == dimsB[1]
                    && dimsA[2] == dimsB[2];
        }

        private class OrientatedItemsComparer : IComparer<OrientatedItem>
        {
            public int widthLeft;
            public int lengthLeft;
            public int depthLeft;
            
            public ItemList nextItems;
            public int rowLength;
            
            public int x;
            public int y;
            public int z;

            public PackedItemList prevPackedItemList;

            private readonly OrientatedItemFactory _oif;

            public OrientatedItemsComparer(OrientatedItemFactory oif)
            {
                _oif = oif;
            }

            public int Compare(OrientatedItem a, OrientatedItem b)
            {
                var orientationAWidthLeft = widthLeft - a.Width;
                var orientationALengthLeft = lengthLeft - a.Length;
                var orientationADepthLeft = depthLeft - a.Depth;

                var orientationBWidthLeft = widthLeft - b.Width;
                var orientationBLengthLeft = lengthLeft - b.Length;
                var orientationBDepthLeft = depthLeft - b.Depth;

                var orientationAMinGap = Math.Min(orientationAWidthLeft, orientationALengthLeft);
                var orientationBMinGap = Math.Min(orientationBWidthLeft, orientationBLengthLeft);

                if (orientationAMinGap == 0)
                {
                    // prefer A if it leaves no gap
                    return -1;
                }
                if (orientationBMinGap == 0)
                {
                    // prefer B if it leaves no gap
                    return 1;
                }

                // prefer leaving room for next item in current row
                if (nextItems.Count() != 0)
                {
                    var itemToCheck = nextItems.First();
                    var nextItemFitA = _oif.GetPossibleOrientations(itemToCheck,
                                                                                    a,
                                                                                    orientationAWidthLeft,
                                                                                    lengthLeft,
                                                                                    depthLeft,
                                                                                    x,
                                                                                    y,
                                                                                    z,
                                                                                    prevPackedItemList
                                                                                ).Count > 0;
                    var nextItemFitB = _oif.GetPossibleOrientations(itemToCheck,
                                                                                    b,
                                                                                    orientationBWidthLeft,
                                                                                    lengthLeft,
                                                                                    depthLeft,
                                                                                    x,
                                                                                    y,
                                                                                    z,
                                                                                    prevPackedItemList
                                                                                ).Count > 0;

                    if (nextItemFitA && !nextItemFitB)
                    {
                        return -1;
                    }
                    if (!nextItemFitA && nextItemFitB)
                    {
                        return 1;
                    }

                    // if not an easy either/or, do a partial lookahead
                    var additionalPackedA = _oif.CalculateAdditionalItemsPackedWithThisOrientation(a,
                                                                                                    nextItems,
                                                                                                    widthLeft,
                                                                                                    lengthLeft,
                                                                                                    depthLeft,
                                                                                                    rowLength
                                                                                                );
                    var additionalPackedB = _oif.CalculateAdditionalItemsPackedWithThisOrientation(b,
                                                                                                    nextItems,
                                                                                                    widthLeft,
                                                                                                    lengthLeft,
                                                                                                    depthLeft,
                                                                                                    rowLength
                                                                                                );

                    if (additionalPackedA != additionalPackedB)
                    {
                        return additionalPackedB.CompareTo(additionalPackedA);
                    }
                }

                // otherwise prefer leaving minimum possible gap, or the greatest footprint
                if (orientationADepthLeft != orientationBDepthLeft)
                {
                    return orientationADepthLeft.CompareTo(orientationBDepthLeft);
                }
                else if (orientationAMinGap != orientationBMinGap)
                {
                    return orientationAMinGap.CompareTo(orientationBMinGap);
                }

                return a.SurfaceFootprint.CompareTo(b.SurfaceFootprint);
            }
        }
    }
}
