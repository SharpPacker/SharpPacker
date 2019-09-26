using SharpPacker.Strategy.BoxPackerClone;
using SharpPacker.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using sharp = SharpPacker.Base.Models;
using clone = SharpPacker.Strategy.BoxPackerClone.Models;
using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Abstract;
using System.Linq;

namespace SharpPacker.Strategy.BoxPackerClone
{
    public class Options
    {
        public int MaxBoxesToBalanceWeight = 12;
    }
    public class BoxPackerCloneStrategy : ABoxPackerStrategy<Options>, IBoxPacker
    {
        public override string StrategyName() => "BoxPackerCloneStrategy";
        public override sharp.BoxPackerResult Pack(sharp.BoxPackerRequest request)
        {
            var packer = new InfalliblePacker
            {
                MaxBoxesToBalanceWeight = options.MaxBoxesToBalanceWeight
            };

            foreach (var box in request.BoxTypes)
            {
                packer.AddBox(Convertors.BoxToBox(box));
            }

            foreach(var set in request.Bundles)
            {
                for (var i = 0; i < set.Quantity; i++)
                {
                    packer.AddItem(Convertors.ItemToItem(set.Item));
                }
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
                UnpackedItems = unpackedItems.GroupBy(item => item).Select(g => new sharp.ItemsBundle(g.Key, (uint)g.Count())),
            };

            return result;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
