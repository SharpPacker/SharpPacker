using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Models
{
    public class PackedBox : IComparable<PackedBox>
    {
        public PackedBox()
        {
            this.PackedItems = new PackedItemList();
        }

        public PackedBox(Box box, PackedItemList pItems)
        {
            this.Box = box;
            this.PackedItems = pItems;
        }

        public Box Box { get; set; }
        public PackedItemList PackedItems { get; set; }

        private bool ItemsIsEmpty => PackedItems.Count == 0;

        public int RemainingDepth => (Box?.InnerDepth ?? 0) - UsedDepth;
        public int RemainingLength => (Box?.InnerLength ?? 0) - UsedLength;
        public int RemainingWidth => (Box?.InnerWidth ?? 0) - UsedWidth;

        public int ItemsWeight => ItemsIsEmpty ? 0 : PackedItems.Sum(item => item.Weight);
        public int TotalWeight => (Box?.EmptyWeight ?? 0) + ItemsWeight;
        public int RemainingWeight => (Box?.MaxWeight ?? 0) - TotalWeight;

        public float InnerVolume => (Box?.InnerVolume ?? 0);
        public float UsedVolume => ItemsIsEmpty ? 0 : PackedItems.Sum(item => item.Volume);
        public float UnusedVolume => InnerVolume - UsedVolume;

        public int UsedDepth => ItemsIsEmpty ? 0 : PackedItems.Max(item => (item.Z + item.Depth));
        public int UsedLength => ItemsIsEmpty ? 0 : PackedItems.Max(item => (item.Y + item.Length));
        public int UsedWidth => ItemsIsEmpty ? 0 : PackedItems.Max(item => (item.X + item.Width));

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

        override public string ToString()
        {
            return $"PackedBox {Box?.Reference} [w{UsedWidth}/{Box?.InnerWidth ?? 0}, l{UsedLength}/{Box?.InnerLength ?? 0}, d{UsedLength}/{Box?.InnerDepth ?? 0}]";
        }

        public int CompareTo(PackedBox other)
        {
            var itemsInThis = this.PackedItems.Count;
            var itemsInOther = other.PackedItems.Count;

            var choise = itemsInOther.CompareTo(itemsInThis);
            if(choise == 0)
            {
                choise = other.VolumeUtilizationPercent.CompareTo(this.VolumeUtilizationPercent);
            }
            if (choise == 0)
            {
                choise = other.UsedVolume.CompareTo(this.UsedVolume);
            }
            if (choise == 0)
            {
                choise = other.TotalWeight.CompareTo(this.TotalWeight);
            }

            return choise;
        }
    }
}
