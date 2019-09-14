using BoxPackerClone;
using SharpPacker.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using sharp = SharpPacker.Core.Models;
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
            Console.WriteLine("BoxPackerCloneAdapter.Init()");
        }

        public override sharp.BoxPackerResult Pack(sharp.BoxPackerRequest request)
        {
            var packer = new Packer();
            packer.MaxBoxesToBalanceWeight = options.MaxBoxesToBalanceWeight;

            foreach(var box in request.Boxes)
            {
                packer.AddBox(Convertors.BoxToBox(box));
            }

            foreach(var item in request.Items)
            {
                packer.AddItem(Convertors.ItemToItem(item));
            }

            clone.PackedBoxList pbl = packer.Pack();

            var packedBoxes = new List<sharp.PackedBox>();
            var unpackedItems = new List<sharp.Item>();

            throw new NotImplementedException();

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
            Console.WriteLine($"BoxPackerCloneAdapter.Dispose({disposing})");
        }
    }
}
