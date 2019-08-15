﻿using SharpPacker.Helpers;
using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpPacker.Services
{
    class WeightRedistributor
    {
        private readonly List<Box4d> boxes;

        public WeightRedistributor(IEnumerable<Box4d> _boxes)
        {
            boxes = new List<Box4d>();
            boxes.AddRange(_boxes);
        }

        /// <summary>
        /// Attempt to equalise weight distribution between 2 boxes
        /// </summary>
        /// <param name="boxA"></param>
        /// <param name="boxB"></param>
        /// <param name="targetWeight"></param>
        /// <returns>was the weight rebalanced?</returns>
        private bool EqualiseWeight(ref PackedBox4d boxA, ref PackedBox4d boxB, float targetWeight)
        {
            var anyIterationSuccessful = false;

            PackedBox4d overWeightBox;
            PackedBox4d underWeightBox;

            if(boxA.TotalWeight > boxB.TotalWeight)
            {
                overWeightBox = boxA;
                underWeightBox = boxB;
            } else
            {
                overWeightBox = boxB;
                underWeightBox = boxA;
            }

            var overWeightBoxItems = overWeightBox.PackedItems.ToList();
            var underWeightBoxItems = underWeightBox.PackedItems.ToList();

            var i = overWeightBoxItems.Count - 1;
            while(i >= 0)
            {
                var overWeightItem = overWeightBoxItems[i];
                i--;

                throw new Exception("check algorithm logic - why there is direct boxB using instead of over/underWeightBox?");
                if(overWeightItem.Weight + boxB.TotalWeight > targetWeight)
                {
                    continue;
                }

                var newLighterBoxes = DoVolumeRepack(underWeightBoxItems, overWeightItem);
                if(newLighterBoxes.Count != 1)
                {
                    continue; //only want to move this item if it still fits in a single box
                }

                underWeightBoxItems.Add(overWeightItem);

                if(overWeightBoxItems.Count == 1)
                { //sometimes a repack can be efficient enough to eliminate a box
                    boxB = newLighterBoxes[0];
                    boxA = null;

                    return true;
                } else
                {
                    overWeightBoxItems.RemoveAt(i+1);
                    var newHeavierBoxes = DoVolumeRepack(overWeightBoxItems, null);
                    if (newHeavierBoxes.Count != 1)
                    {
                        continue; //only want to move this item if it still fits in a single box
                    }

                    if (DidRepackActuallyHelp(boxA, boxB, newHeavierBoxes[0], newLighterBoxes[0]))
                    {
                        boxB = newLighterBoxes[0];
                        boxA = newHeavierBoxes[0];
                        anyIterationSuccessful = true;
                    }
                }

            }
            return anyIterationSuccessful;
        }

        /// <summary>
        /// Do a volume repack of a set of items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private List<PackedBox4d> DoVolumeRepack(IEnumerable<Item4d> items)
        {
            return DoVolumeRepack(items, null);
        }

        /// <summary>
        /// Do a volume repack of a set of items.
        /// </summary>
        /// <param name="originalItems"></param>
        /// <param name="plusOneItem"></param>
        /// <returns></returns>
        private List<PackedBox4d> DoVolumeRepack(IEnumerable<Item4d> originalItems, Item4d plusOneItem)
        {
            var packer = new Packer();
            packer.SetBoxes(this.boxes);
            packer.SetItems(originalItems);
            if(plusOneItem != null)
            {
                packer.AddItem(plusOneItem);
            }

            return packer.DoVolumePacking();
        }
        
        /// <summary>
        /// Do a volume repack of a set of items.
        /// </summary>
        /// <param name="originalPackedItems"></param>
        /// <param name="plusOnePackedItem"></param>
        /// <returns></returns>
        private List<PackedBox4d> DoVolumeRepack(IEnumerable<PackedItem4d> originalPackedItems, PackedItem4d plusOnePackedItem)
        {
            return DoVolumeRepack(originalPackedItems.Select(pi => pi.Item), plusOnePackedItem?.Item);
        }


        /// <summary>
        /// Not every attempted repack is actually helpful - sometimes moving an item between two otherwise identical
        /// boxes, or sometimes the box used for the now lighter set of items actually weighs more when empty causing
        /// an increase in total weight.
        /// </summary>
        /// <returns></returns>
        private bool DidRepackActuallyHelp(PackedBox4d oldBoxA, PackedBox4d oldBoxB, PackedBox4d newBoxA, PackedBox4d newBoxB)
        {
            PackedBox4d[] oldList = { oldBoxA, oldBoxB };
            PackedBox4d[] newList = { newBoxA, newBoxB };

            var oldWeigthVariance = PackedBoxListHelpers.GetWeightVariance(oldList);
            var newWeigthVariance = PackedBoxListHelpers.GetWeightVariance(newList);

            return (newWeigthVariance < oldWeigthVariance);
        }
    }
}
