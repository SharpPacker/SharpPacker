using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Services
{
    internal class VolumePacker
    {
        /// <summary>
        /// Box to pack items into.
        /// </summary>
        private readonly Box4d box;

        /// <summary>
        /// List of items to be packed.
        /// </summary>
        private readonly List<Item4d> items;

        /// <summary>
        /// List of items temporarily skipped to be packed.
        /// </summary>
        private readonly List<Item4d> skippedItems;

        /// <summary>
        /// Remainig weight capacity of the box.
        /// </summary>
        private int remainingWeight;

        private List<PackedLayer> layers;

        public bool LookAheadMode { get; set; } = false;

        private int BoxWidth => Math.Max(box.InnerWidth, box.InnerLength);
        private int BoxLength => Math.Min(box.InnerWidth, box.InnerLength);
        private bool BoxRotated => (BoxWidth != box.InnerWidth) || (BoxLength != box.InnerLength);

        public VolumePacker(Box4d _box, IEnumerable<Item4d> _items)
        {
            box = _box;
            items = _items.ToList();
            skippedItems = new List<Item4d>();
            remainingWeight = box.MaxWeight - box.EmptyWeight;
            layers = new List<PackedLayer>();
        }

        /// <summary>
        /// Pack as many items as possible into specific given box.
        /// </summary>
        /// <returns></returns>
        public PackedBox4d Pack()
        {
            while (items.Count > 0)
            {
                var layerStartDepth = GetCurrentPackedDepth();
                PackLayer(layerStartDepth, BoxWidth, BoxLength, box.InnerDepth - layerStartDepth);
            }

            if (BoxRotated)
            {
                RotateLayersNinetyDegrees();
            }

            if (LookAheadMode)
            {
                StabiliseLayers();
            }

            var result = new PackedBox4d()
            {
                Box = box,
                PackedItems = GetPackedItemList()
            };

            return result;
        }

        /// <summary>
        /// During packing, it is quite possible that layers have been created that aren't physically stable i.e.they overhang the ones below.
        /// This function reorders them so that the ones with the greatest surface area are placed at the bottom
        /// </summary>
        public void StabiliseLayers()
        {
            var stabeliser = new LayerStabiliser();
            layers = stabeliser.stabilise(layers);
        }

        private OrientatedItem4d GetOrientationForItem(
                                    Item4d itemToPack,
                                    PackedItem4d prevItem,
                                    IEnumerable<Item4d> nextItems,
                                    bool isLastItem,
                                    int maxWidth,
                                    int maxLength,
                                    int maxDepth,
                                    int rowLength,
                                    int x,
                                    int y,
                                    int z
                                )
        {
            var prevOrientatedItem = prevItem?.ToOrientatedItem();

            // TODO: ConstrainedPlacementItem check implementation
            var prevPackedItemList = GetPackedItemList();

            var oif = new OrientatedItemFactory(box);
            var oifDecision = oif.GetBestOrientation(
                                itemToPack,
                                prevOrientatedItem,
                                nextItems,
                                isLastItem,
                                maxWidth,
                                maxLength,
                                maxDepth,
                                rowLength,
                                x,
                                y,
                                z,
                                prevPackedItemList
                            );

            return oifDecision;
        }

        private void TryAndStackItemsIntoSpace(
                        PackedLevel layer,
                        PackedItem4d prevItem,
                        IEnumerable<Item4d> nextItems,
                        int maxWidth,
                        int maxLength,
                        int maxDepth,
                        int x,
                        int y,
                        int z,
                        int rowLength
                    )
        {
            while (items.Count() > 0)
            {
                var firstItem = items.First();
                var thisIsLastItem = (items.Count() == 1);
                if (CheckNonDimensionalConstrains(firstItem))
                {
                    break;
                }

                var stackedItem = GetOrientationForItem(
                                        firstItem,
                                        prevItem,
                                        nextItems,
                                        thisIsLastItem,
                                        maxWidth,
                                        maxLength,
                                        maxDepth,
                                        rowLength,
                                        x,
                                        y,
                                        z
                                    );

                if (stackedItem != null)
                {
                    layer.Insert(PackedItem4d.FromOrientatedItem(stackedItem, x, y, z));

                    remainingWeight -= firstItem.Weight;
                    maxDepth -= stackedItem.Depth;
                    z += stackedItem.Depth;

                    items.Remove(firstItem);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Check item generally fits into box.
        /// </summary>
        /// <param name="itemToPack"></param>
        /// <returns></returns>
        private bool CheckConstrains(Item4d itemToPack)
        {
            return CheckNonDimensionalConstrains(itemToPack) &&
                CheckDimensionalConstrains(itemToPack);
        }

        /// <summary>
        /// As well as purely dimensional constraints, there are other constraints that need to be met
        /// e.g.weight limits or item-specific restrictions(e.g.max<x> batteries per box).
        /// </summary>
        /// <param name="itemToPack"></param>
        /// <returns></returns>
        private bool CheckNonDimensionalConstrains(Item4d itemToPack)
        {
            var weightOk = itemToPack.Weight <= remainingWeight;
            // TODO: Add ConstrainedItem restrictions check

            return weightOk;
        }

        /// <summary>
        /// Check the item physically fits in the box (at all).
        /// </summary>
        /// <param name="itemToPack"></param>
        /// <returns></returns>
        private bool CheckDimensionalConstrains(Item4d itemToPack)
        {
            var oif = new OrientatedItemFactory(box);
            return oif.getPossibleOrientationsInEmptyBox(itemToPack);
        }

        /// <summary>
        /// Reintegrate skipped items into main list.
        /// </summary>
        private void RebuildItemList(Item4d currentItem = null)
        {
            if (items.Count() == 0)
            {
                items.AddRange(skippedItems);
                skippedItems.Clear();
            }

            if (currentItem != null)
            {
                items.Add(currentItem);
            }
        }

        /// <summary>
        /// Swap back width/length of the packed items to match orientation of the box if needed.
        /// </summary>
        private void RotateLayersNinetyDegrees()
        {
            foreach (var originalLayer in layers)
            {
                foreach (var item in originalLayer.Items)
                {
                    var oldX = item.X;
                    item.X = item.Y;
                    item.Y = oldX;

                    var oldWidth = item.Width;
                    item.Width = item.Length;
                    item.Length = oldWidth;
                }
            }
        }

        /// <summary>
        /// Are there items left to pack?
        /// </summary>
        /// <returns></returns>
        private bool HasItemsLeftToPack()
        {
            return skippedItems.Count() + items.Count() == 0;
        }

        /// <summary>
        /// Generate a single list of items packed.
        /// </summary>
        /// <returns></returns>
        private List<PackedItem4d> GetPackedItemList()
        {
            var packedItemList = new List<PackedItem4d>();
            foreach (var layer in layers)
            {
                foreach (var packedItem in layer.Items)
                {
                    packedItemList.Add(packedItem);
                }
            }

            return packedItemList;
        }

        /// <summary>
        /// Return the current packed depth.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentPackedDepth()
        {
            var depth = layers.Sum(l => l.Depth);
            return depth;
        }
    }
}