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

        public OrientatedItem4d GetBestOrientation(
                               Item4d item,
                               OrientatedItem4d prevItem,
                               IEnumerable<Item4d> nextItems,
                               bool isLastItem,
                               int widthLeft,
                               int lengthLeft,
                               int depthLeft,
                               int rowLength,
                               int x,
                               int y,
                               int z,
                               IEnumerable<PackedItem4d> prevPackedItemList
                           )
        {
            var possibleOrientations = GetPossibleOrientations(item, prevItem, widthLeft, lengthLeft, depthLeft, x, y, z, prevPackedItemList);
            var usableOrientations = GetUsableOrientations(item, possibleOrientations, isLastItem);

            if (usableOrientations.Count == 0)
            {
                return null;
            }

            var comparer = new OrientatedItemsComparer(this)
            {
                widthLeft = widthLeft,
                lengthLeft = lengthLeft,
                depthLeft = depthLeft,
                nextItems = nextItems.ToList(),
                rowLength = rowLength,
                x = x,
                y = y,
                z = z,
                prevPackedItemList = prevPackedItemList.ToList(),
            };
            usableOrientations.Sort(comparer);

            var bestFit = usableOrientations.First();

            return bestFit;
        }

        public List<OrientatedItem4d> GetPossibleOrientations(Item4d item,
            OrientatedItem4d prevItem,
            int widthLeft,
            int lengthLeft,
            int depthLeft,
            int x,
            int y,
            int z,
            IEnumerable<PackedItem4d> prevPackedItemsList)
        {
            var orientations = new List<OrientatedItem4d>();

            if (prevItem != null && IsSameDimensions(prevItem.Item, item))
            {
                // Special case items that are the same as what we just packed - keep orientation
                orientations.Add(
                    new OrientatedItem4d()
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
                    new OrientatedItem4d()
                    {
                        Item = item,
                        Width = item.Width,
                        Length = item.Length,
                        Depth = item.Depth
                    }
                );
                orientations.Add(
                    new OrientatedItem4d()
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
                        new OrientatedItem4d()
                        {
                            Item = item,
                            Width = item.Width,
                            Length = item.Depth,
                            Depth = item.Length
                        }
                    );

                    orientations.Add(
                        new OrientatedItem4d()
                        {
                            Item = item,
                            Width = item.Length,
                            Length = item.Depth,
                            Depth = item.Width
                        }
                    );

                    orientations.Add(
                        new OrientatedItem4d()
                        {
                            Item = item,
                            Width = item.Depth,
                            Length = item.Width,
                            Depth = item.Length
                        }
                    );

                    orientations.Add(
                        new OrientatedItem4d()
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

        public List<OrientatedItem4d> GetPossibleOrientationsInEmptyBox(Item4d item)
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
                                                        new List<PackedItem4d>()
                                                    );

            return orientations;
        }

        public List<OrientatedItem4d> GetUsableOrientations(Item4d item, IEnumerable<OrientatedItem4d> possibleOrientations, bool isLastItem)
        {
            var orientationsToUse = new List<OrientatedItem4d>();
            var stableOrientations = new List<OrientatedItem4d>();
            var unstableOrientations = new List<OrientatedItem4d>();

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

        protected int CalculateAdditionalItemsPackedWithThisOrientation(OrientatedItem4d prevItem,
                                                                        IEnumerable<Item4d> nextItems,
                                                                        int originalWidthLeft,
                                                                        int originalLengthLeft,
                                                                        int depthLeft,
                                                                        int currentRowLengthBeforePacking)
        {
            var currentRowLength = Math.Max(prevItem.Length, currentRowLengthBeforePacking);

            // cap lookahead as this gets recursive and slow
            var capThreshold = 8;
            var itemsToPack = nextItems.OrderByDescending(i => i).Take(capThreshold).Reverse();

            var tempBox = new WorkingVolume4d(originalWidthLeft - prevItem.Width, currentRowLength, depthLeft, int.MaxValue);
            var tempPacker = new VolumePacker(tempBox, itemsToPack.ToList())
            {
                LookAheadMode = true
            };
            // TODO Broken here, TestIssue174
            var remainigRowPacked = tempPacker.Pack();

            itemsToPack = itemsToPack.Except(remainigRowPacked.PackedItems.Select(x => x.Item));

            return nextItems.Count() - itemsToPack.Count();
        }

        /// <summary>
        /// Return the orientations for this item if it were to be placed into the box with nothing else.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private List<OrientatedItem4d> GetStableOrientationsInEmptyBox(Item4d item)
        {
            var orientations = GetPossibleOrientationsInEmptyBox(item);
            return orientations.Where(o => o.IsStable()).ToList();
        }

        private List<OrientatedItem4d> GetStablePrientationsInEmptyBox(Item4d item)
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
        private bool IsSameDimensions(Item4d itemA, Item4d itemB)
        {
            var dimsA = new int[] { itemA.Width, itemA.Length, itemA.Depth };
            var dimsB = new int[] { itemB.Width, itemB.Length, itemB.Depth };

            dimsA.OrderBy(d => d);
            dimsB.OrderBy(d => d);

            return dimsA[0] == dimsB[0]
                    && dimsA[1] == dimsB[1]
                    && dimsA[2] == dimsB[2];
        }

        private class OrientatedItemsComparer : IComparer<OrientatedItem4d>
        {
            public int depthLeft;
            public int lengthLeft;
            public IEnumerable<Item4d> nextItems;
            public List<PackedItem4d> prevPackedItemList;
            public int rowLength;
            public int widthLeft;
            public int x;
            public int y;
            public int z;
            private readonly OrientatedItemFactory _oif;

            public OrientatedItemsComparer(OrientatedItemFactory oif)
            {
                _oif = oif;
            }

            public int Compare(OrientatedItem4d a, OrientatedItem4d b)
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
                    var nextItemFitOrientationsA = _oif.GetPossibleOrientations(itemToCheck,
                                                                                    a,
                                                                                    orientationAWidthLeft,
                                                                                    lengthLeft,
                                                                                    depthLeft,
                                                                                    x,
                                                                                    y,
                                                                                    z,
                                                                                    prevPackedItemList
                                                                                );
                    var nextItemFitOrientationsB = _oif.GetPossibleOrientations(itemToCheck,
                                                                                    b,
                                                                                    orientationAWidthLeft,
                                                                                    lengthLeft,
                                                                                    depthLeft,
                                                                                    x,
                                                                                    y,
                                                                                    z,
                                                                                    prevPackedItemList
                                                                                );

                    if (nextItemFitOrientationsA.Count() != 0 && nextItemFitOrientationsB.Count() == 0)
                    {
                        return -1;
                    }
                    if (nextItemFitOrientationsA.Count() == 0 && nextItemFitOrientationsB.Count() != 0)
                    {
                        return 1;
                    }

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
