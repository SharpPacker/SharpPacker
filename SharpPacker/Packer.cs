using SharpPacker.Exceptions;
using SharpPacker.Models;
using SharpPacker.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker
{
    public class Packer : IPacker
    {
        public int MaxBoxesToBalanceWeight { get; set; } = 12;

        private ItemList _items;
        private BoxList _boxes;

        public Packer()
        {
            _items = new ItemList();
            _boxes = new BoxList();
        }

        public void AddItem(Item item, int quantity = 1)
        {
            for(var i = 0; i < quantity; i++)
            {
                this._items.Insert(item);
            }
        }

        public void SetItems(ItemList itemCollection)
        {
            _items = itemCollection;
        }

        public void SetItems(IEnumerable<Item> itemCollection)
        {
            _items = new ItemList();
            foreach(var item in itemCollection)
            {
                _items.Insert(item);
            }
        }

        public void AddBox(Box box)
        {
            _boxes.Insert(box);
        }

        public void SetBoxes(BoxList boxCollection)
        {
            _boxes = boxCollection;
        }

        public PackedBoxList DoVolumePacking()
        {
            var packedBoxes = new PackedBoxList();

            //Keep going until everything packed
            while(_items.Count > 0)
            {
                var packedBoxesIteration = new List<PackedBox>();

                //Loop through boxes starting with smallest, see what happens
                foreach(var box in _boxes)
                {
                    var volumePacker = new VolumePacker(box, _items.Clone());
                    var packedBox = volumePacker.Pack();

                    if (packedBox.PackedItems.Count != 0)
                    {
                        packedBoxesIteration.Add(packedBox);

                        //Have we found a single box that contains everything?
                        if (packedBox.PackedItems.Count == _items.Count)
                        {
                            break;
                        }
                    }
                }
                // if any items is packed, then any chanses for this in next iteration
                if (packedBoxesIteration.Count == 0)
                {
                    throw new ItemTooLargeException();
                }
                //Find best box of iteration, and remove packed items from unpacked list
                var bestBox = FindBestBoxFromIteration(packedBoxesIteration);

                foreach (var itemToRemove in bestBox.PackedItems)
                {
                    _items.Remove(itemToRemove.Item);
                }

                packedBoxes.Insert(bestBox);
            }

            return packedBoxes;
        }

        public PackedBoxList Pack()
        {
            var packedBoxes = DoVolumePacking();

            //If we have multiple boxes, try and optimise/even-out weight distribution
            if(packedBoxes.Count > 1 && packedBoxes.Count <= MaxBoxesToBalanceWeight)
            {
                var redistributor = new WeightRedistributor(_boxes);
                packedBoxes = redistributor.RedistributeWeight(packedBoxes);
            }

            return packedBoxes;
        }

        private PackedBox FindBestBoxFromIteration(List<PackedBox> packedBoxes)
        {
            packedBoxes.Sort(Compare);

            var result = packedBoxes.FirstOrDefault();
            if(packedBoxes.Count > 0)
            {
                packedBoxes.RemoveAt(0);
            }
            return result;
        }

        private int Compare(PackedBox boxA, PackedBox boxB)
        {
            var itemsInThis = boxA.PackedItems.Count;
            var itemsInOther = boxB.PackedItems.Count;

            var choise = itemsInOther.CompareTo(itemsInThis);
            if (choise == 0)
            {
                choise = boxB.VolumeUtilizationPercent.CompareTo(boxA.VolumeUtilizationPercent);
            }
            if (choise == 0)
            {
                choise = boxB.UsedVolume.CompareTo(boxA.UsedVolume);
            }
            if (choise == 0)
            {
                choise = boxB.TotalWeight.CompareTo(boxA.TotalWeight);
            }

            return choise;
        }
    }
}
