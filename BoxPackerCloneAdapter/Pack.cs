using BoxPackerClone;
using SharpPacker.Core;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;

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
        }

        public override void Init(Options options)
        {
            this.options = options;
            Console.WriteLine("BoxPackerCloneAdapter.Init()");
        }

        public override BoxPackerResult PackBoxes(IEnumerable<BoxType> boxes, IEnumerable<Item> items)
        {
            var packer = new Packer();
            packer.MaxBoxesToBalanceWeight = options.MaxBoxesToBalanceWeight;

            foreach(var box in boxes)
            {
                packer.AddBox(Convertors.BoxToBox(box));
            }

            foreach(var item in items)
            {
                packer.AddItem(Convertors.ItemToItem(item));
            }

            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine($"BoxPackerCloneAdapter.Dispose({disposing})");
        }
    }
}
