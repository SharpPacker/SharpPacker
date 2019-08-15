using SharpPacker.Helpers;
using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Services
{
    class WeightRedistributor
    {
        public List<Box4d> Boxes { get; set; }

        public WeightRedistributor(IEnumerable<Box4d> _boxes)
        {
            Boxes = new List<Box4d>();
            Boxes.AddRange(_boxes);
        }

        /// <summary>
        /// Do a volume repack of a set of items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private List<PackedBox4d> DoVolumeRepack(IEnumerable<Item4d> items)
        {
            var packer = new Packer();
            packer.SetBoxes(this.Boxes);
            packer.SetItems(items);

            return packer.DoVolumePacking();
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
