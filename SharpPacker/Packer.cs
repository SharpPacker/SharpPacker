using SharpPacker.Exceptions;
using SharpPacker.Models;
using SharpPacker.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker
{
    public class Packer : IPacker<Item4d, Box4d, PackedBox4d>
    {
        public int MaxBoxesToBalanceWeight { get; set; } = 12;

        private readonly List<Item4d> _items;
        private readonly List<Box4d> _boxes;

        public Packer()
        {
            _items = new List<Item4d>();
            _boxes = new List<Box4d>();
        }

        public void AddItem(Item4d item, int quantity = 1)
        {
            for(var i = 0; i < quantity; i++)
            {
                this._items.Add(item);
            }
        }

        public void SetItems(IEnumerable<Item4d> itemCollection)
        {
            _items.Clear();
            _items.AddRange(itemCollection);
        }

        public void AddBox(Box4d box)
        {
            _boxes.Add(box);
        }

        public void SetBoxes(IEnumerable<Box4d> boxCollection)
        {
            _boxes.Clear();
            _boxes.AddRange(boxCollection);
        }

        public List<PackedBox4d> DoVolumePacking()
        {
            var packedBoxes = new List<PackedBox4d>();

            //Keep going until everything packed
            var itemsToPack = _items.ToList();
            while(itemsToPack.Count > 0)
            {
                var packedBoxesIteration = new List<PackedBox4d>();

                //Loop through boxes starting with smallest, see what happens
                foreach(var box in _boxes)
                {
                    var volumePacker = new VolumePacker(box, itemsToPack.ToList());
                    var packedBox = volumePacker.Pack();

                    var packedItemsCount = packedBox.PackedItems.Count;
                    if (packedItemsCount != 0)
                    {
                        packedBoxesIteration.Add(packedBox);

                        if(packedItemsCount == itemsToPack.Count)
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
                packedBoxes.Add(bestBox);
                foreach (var itemToRemove in bestBox.PackedItems)
                {
                    itemsToPack.Remove(itemToRemove.Item);
                }
            }

            return packedBoxes;
        }

        public List<PackedBox4d> Pack()
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

        private PackedBox4d FindBestBoxFromIteration(IEnumerable<PackedBox4d> packedBoxes)
        {
            packedBoxes.OrderBy(pb => pb);
            return packedBoxes.First();
        }


    }
}
