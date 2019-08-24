using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Models
{
    public class PackedBox4d
    {
        public PackedBox4d()
        {
            this.PackedItems = new List<PackedItem4d>();
        }

        public PackedBox4d(Box4d box, IEnumerable<PackedItem4d> pItems)
        {
            this.Box = box;
            this.PackedItems = pItems.ToList();
        }

        public Box4d Box { get; set; }
        public int ItemsWeight => PackedItems.Sum(item => item.Weight);
        public List<PackedItem4d> PackedItems { get; set; }
        public int RemainingDepth => Box.InnerDepth - UsedDepth;
        public int RemainingLength => Box.InnerLength - UsedLength;
        public int RemainingWeight => Box.MaxWeight - TotalWeight;
        public int RemainingWidth => Box.InnerWidth - UsedWidth;
        public int TotalWeight => Box.EmptyWeight + ItemsWeight;
        public float UnusedVolume => InnerVolume - UsedVolume;
        public int UsedDepth => PackedItems.Max(item => (item.Z + item.Depth));
        public int UsedLength => PackedItems.Max(item => (item.Y + item.Length));
        public float UsedVolume => PackedItems.Sum(item => item.Volume);
        public int UsedWidth => PackedItems.Max(item => (item.X + item.Width));

        public float VolumeUtilizationPercent {
            get {
                if (InnerVolume == 0)
                {
                    return 0;
                }
                else
                {
                    return (float)Math.Round(100 * (double)UsedVolume / InnerVolume, 2);
                }
            }
        }

        public float InnerVolume => (Box.InnerVolume);
    }
}
