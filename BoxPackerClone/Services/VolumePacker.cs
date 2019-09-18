using BoxPackerClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SharpPacker.Tests")]

namespace BoxPackerClone.Services
{
    public class VolumePacker
    {
        /// <summary>
        /// Box to pack items into.
        /// </summary>
        private readonly Box box;

        /// <summary>
        /// List of items to be packed.
        /// </summary>
        private readonly ItemList items;

        /// <summary>
        /// List of items temporarily skipped to be packed.
        /// </summary>
        private readonly List<Item> skippedItems;

        private List<PackedLayer> layers;

        /// <summary>
        /// Remainig weight capacity of the box.
        /// </summary>
        private int remainingWeight;

        private readonly OrientatedItemFactory orientatedItemFactory;

        public VolumePacker(Box _box, ItemList _items)
        {
            box = _box;
            items = _items;
            skippedItems = new List<Item>();
            remainingWeight = box.MaxWeight - box.EmptyWeight;
            layers = new List<PackedLayer>();
            orientatedItemFactory = new OrientatedItemFactory(box);
        }

        public bool LookAheadMode { get; set; } = false;

        private int BoxLength => Math.Min(box.InnerWidth, box.InnerLength);
        private int BoxWidth => Math.Max(box.InnerWidth, box.InnerLength);
        private bool BoxRotated => (BoxWidth != box.InnerWidth) || (BoxLength != box.InnerLength);

        /// <summary>
        /// Pack as many items as possible into specific given box.
        /// </summary>
        /// <returns></returns>
        public PackedBox Pack()
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

            if (!LookAheadMode)
            {
                StabiliseLayers();
            }

            var result = new PackedBox()
            {
                Box = box,
                PackedItems = GetPackedItemList()
            };

            return result;
        }

        /// <summary>
        /// During packing, it is quite possible that layers have been created that aren't physically
        /// stable i.e.they overhang the ones below. This function reorders them so that the ones
        /// with the greatest surface area are placed at the bottom
        /// </summary>
        public void StabiliseLayers()
        {
            var stabeliser = new LayerStabiliser();
            layers = stabeliser.Stabilise(layers);
        }

        /// <summary>
        /// Pack items into an individual vertical layer.
        /// </summary>
        /// <param name="startDepth"></param>
        /// <param name="widthLeft"></param>
        /// <param name="lengthLeft"></param>
        /// <param name="depthLeft"></param>
        protected void PackLayer(int startDepth, int widthLeft, int lengthLeft, int depthLeft)
        {
            var newLayer = new PackedLayer();
            layers.Add(newLayer);

            int x, y, rowWidth, rowLength, layerDepth;
            x = y = rowWidth = rowLength = layerDepth = 0;

            PackedItem prevItem = null;

            while (items.Count > 0)
            {
                var itemToPack = items.Extract();

                //skip items that are simply too heavy or too large
                if (!CheckNonDimensionalConstraints(itemToPack))
                {
                    RebuildItemList();
                    continue;
                }

                var orientedItem = GetOrientationForItem(itemToPack,
                                                            prevItem,
                                                            items,
                                                            !HasItemsLeftToPack(),
                                                            widthLeft,
                                                            lengthLeft,
                                                            depthLeft,
                                                            rowLength,
                                                            x,
                                                            y,
                                                            startDepth
                                                        );

                if (orientedItem != null)
                {
                    var packedItem = PackedItem.FromOrientatedItem(orientedItem, x, y, startDepth);
                    newLayer.Insert(packedItem);
                    remainingWeight -= orientedItem.Item.Weight;
                    widthLeft -= orientedItem.Width;

                    rowWidth += orientedItem.Width;
                    rowLength = Math.Max(rowLength, orientedItem.Length);
                    layerDepth = Math.Max(layerDepth, orientedItem.Depth);

                    //allow items to be stacked in place within the same footprint up to current layer depth
                    var stackableDepth = layerDepth - orientedItem.Depth;
                    TryAndStackItemsIntoSpace(newLayer,
                                                prevItem,
                                                items,
                                                orientedItem.Width,
                                                orientedItem.Length,
                                                stackableDepth,
                                                x,
                                                y,
                                                startDepth + orientedItem.Depth,
                                                rowLength
                                            );
                    x += orientedItem.Width;

                    prevItem = packedItem;
                    if(items.Count == 0)
                    {
                        RebuildItemList();
                    }
                }
                else if (newLayer.Items.Count == 0)
                {
                    // zero items on layer
                    // doesn't fit on layer even when empty, skipping for good
                    continue;
                }
                else if (widthLeft > 0 && items.Count > 0)
                {
                    // skip for now, move on to the next item
                    skippedItems.Add(itemToPack);

                    // abandon here if next item is the same, no point trying to keep going. Last time is not skipped, need that to trigger appropriate reset logic
                    while(items.Count > 2 && orientatedItemFactory.IsSameDimensions(itemToPack, items.Top()))
                    {
                        this.skippedItems.Add(items.Extract());
                    }
                }
                else if (x > 0 && lengthLeft >= Math.Min(Math.Min(itemToPack.Width, itemToPack.Length), itemToPack.Depth))
                {
                    // No more fit in width wise, resetting for new row
                    widthLeft += rowWidth;
                    lengthLeft -= rowLength;
                    y += rowLength;
                    x = rowWidth = rowLength = 0;
                    skippedItems.Add(itemToPack);
                    RebuildItemList();
                    prevItem = null;
                    continue;
                }
                else
                {
                    // no items fit, so starting next vertical layer
                    skippedItems.Add(itemToPack);
                    RebuildItemList();
                    return;
                }
            }
        }

        /// <summary> As well as purely dimensional constraints, there are other constraints that
        /// need to be met e.g.weight limits or item-specific restrictions(e.g.max<x> batteries per
        /// box). </summary> <param name="itemToPack"></param> <returns></returns>
        private bool CheckNonDimensionalConstraints(Item itemToPack)
        {
            var weightOk = itemToPack.Weight <= remainingWeight;

            // TODO: implement ConstrainedItem check
            return weightOk;
        }

        /// <summary>
        /// Return the current packed depth.
        /// </summary>
        /// <returns></returns>
        private int GetCurrentPackedDepth()
        {
            var depth = layers.Sum(l => l.GetDepth());
            return depth;
        }

        private OrientatedItem GetOrientationForItem(Item itemToPack,
                                    PackedItem prevItem,
                                    ItemList nextItems,
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
            // don't calculate it if not going to be used
            var prevPackedItemList = new PackedItemList();

            var oifDecision = orientatedItemFactory.GetBestOrientation(
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

        /// <summary>
        /// Generate a single list of items packed.
        /// </summary>
        /// <returns></returns>
        private PackedItemList GetPackedItemList()
        {
            var packedItemList = new PackedItemList();
            foreach (var layer in layers)
            {
                foreach (var packedItem in layer.Items)
                {
                    packedItemList.Insert(packedItem);
                }
            }

            return packedItemList;
        }

        /// <summary>
        /// Are there items left to pack?
        /// </summary>
        /// <returns></returns>
        private bool HasItemsLeftToPack()
        {
            return skippedItems.Count() + items.Count() > 0;
        }

        /// <summary>
        /// Reintegrate skipped items into main list.
        /// </summary>
        private void RebuildItemList()
        {
            foreach(var item in skippedItems)
            {
                items.Insert(item);
            }

            skippedItems.Clear();
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

        private void TryAndStackItemsIntoSpace(PackedLayer layer,
                                                PackedItem prevItem,
                                                ItemList nextItems,
                                                int maxWidth,
                                                int maxLength,
                                                int maxDepth,
                                                int x,
                                                int y,
                                                int z,
                                                int rowLength
                                            )
        {
            while (items.Count() > 0 && CheckNonDimensionalConstraints(items.Top()))
            {
                var stackedItem = GetOrientationForItem(
                                        items.Top(),
                                        prevItem,
                                        nextItems,
                                        items.Count == 1,
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
                    remainingWeight -= items.Top().Weight;

                    layer.Insert(PackedItem.FromOrientatedItem(stackedItem, x, y, z));
                    items.Extract();

                    maxDepth -= stackedItem.Depth;
                    z += stackedItem.Depth;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
