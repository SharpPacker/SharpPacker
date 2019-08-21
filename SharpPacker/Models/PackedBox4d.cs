using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Models
{
    public class PackedBox4d
    {
        public Box4d Box { get; set; }
        public List<PackedItem4d> PackedItems { get; set; }

        public int ItemsWeight => PackedItems.Sum(item => item.Weight);
        public int TotalWeight => Box.EmptyWeight + ItemsWeight;

        public int UsedWidth => PackedItems.Max(item => (item.X + item.Width));
        public int UsedLength => PackedItems.Max(item => (item.Y + item.Length));
        public int UsedDepth => PackedItems.Max(item => (item.Z + item.Depth));

        public int RemainingWidth => Box.InnerWidth - UsedWidth;
        public int RemainingLength => Box.InnerLength - UsedLength;
        public int RemainingDepth => Box.InnerDepth - UsedDepth;

        public float InnerVolume => (Box.InnerVolume);
        public float UsedVolume => PackedItems.Sum(item => item.Volume);
        public float UnusedVolume => InnerVolume - UsedVolume;

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
    }
}