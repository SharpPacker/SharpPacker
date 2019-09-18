using BoxPackerClone;
using SharpPacker.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using sharp = SharpPacker.Base.Models;
using clone = BoxPackerClone.Models;


namespace BoxPackerCloneAdapter
{
    public class Options
    {
        public int MaxBoxesToBalanceWeight = 12;
    }

    public class BoxPackerCloneAdapter : ABoxPacker<Options>
    {
        readonly Packer packer = new Packer();
        Options options;

        public BoxPackerCloneAdapter()
        {
            options = new Options();
        }

        public override void Init(Options options)
        {
            this.options = options;
        }

        public override sharp.BoxPackerResult Pack(sharp.BoxPackerRequest request)
        {
            var packer = new Packer
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

            var unpackedItems = new List<sharp.Item>(request.Items);
            foreach (var pBox in packedBoxes)
            {
                foreach(var pItem in pBox.PackedItems)
                {
                    unpackedItems.Remove(pItem.Item);
                }
            }

            var result = new sharp.BoxPackerResult
                {
                    PackedBoxes = packedBoxes,
                    UnpackedItems = unpackedItems
                };

            return result;
        }

        public override async Task<sharp.BoxPackerResult> PackAsync(sharp.BoxPackerRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() => Pack(request));
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
