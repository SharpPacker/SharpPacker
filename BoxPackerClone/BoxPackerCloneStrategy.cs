using BoxPackerClone;
using SharpPacker.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using sharp = SharpPacker.Base.Models;
using clone = BoxPackerClone.Models;
using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Abstract;

namespace BoxPackerClone.Adapter
{
    public class Options
    {
        public int MaxBoxesToBalanceWeight = 12;
    }

    public class BoxPackerCloneStrategy : ABoxPackerStrategy<Options>, IBoxPackerStrategy
    {
        public override sharp.BoxPackerResult Pack(sharp.BoxPackerRequest request)
        {
            var packer = new InfalliblePacker
            {
                MaxBoxesToBalanceWeight = options.MaxBoxesToBalanceWeight
            };

            foreach (var box in request.Boxes)
            {
                packer.AddBox(Convertors.BoxToBox(box));
            }

            foreach(var item in request.Items)
            {
                packer.AddItem(Convertors.ItemToItem(item));
            }

            clone.PackedBoxList pbl = packer.Pack();

            
            var packedBoxes = new List<sharp.PackedBox>
            {
                Capacity = pbl.Count
            };

            foreach (var pBoxFromResult in pbl)
            {
                List<sharp.PackedItem> pItems = new List<sharp.PackedItem>();

                foreach (var pItemFromResult in pBoxFromResult.PackedItems)
                {
                    pItems.Add(Convertors.PackedItemToPackedItem(pItemFromResult));
                }

                var pBox = new sharp.PackedBox
                {
                    Box = Convertors.BoxToBox(pBoxFromResult.Box),
                    PackedItems = pItems,
                };

                packedBoxes.Add(pBox);
            }

            var unpackedItems = new List<sharp.Item>();
            foreach (var unpackedItem in packer._unpackedItems)
            {
                unpackedItems.Add(Convertors.ItemToItem(unpackedItem));
            }

            var result = new sharp.BoxPackerResult
                {
                    PackedBoxes = packedBoxes,
                    UnpackedItems = unpackedItems
                };

            return result;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
