using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SharpPacker.Models
{
    internal class PackedLayer : IComparable<PackedLayer>
    {
        public List<PackedItem4d> Items { get; set; } = new List<PackedItem4d>();

        /// <summary>
        /// Add a packed item to this layer.
        /// </summary>
        /// <param name="packedItem"></param>
        public void Insert(PackedItem4d packedItem)
        {
            this.Items.Add(packedItem);
        }

        /// <summary>
        /// Calculate footprint area of this layer.
        /// </summary>
        /// <returns></returns>
        public int GetFootprint()
        {
            var layerWidth = Items.Max(i => i.X + i.Width);
            var layerLength = Items.Max(i => i.Y + i.Length);

            return layerWidth * layerLength;
        }

        /// <summary>
        /// Calculate start depth of this layer.
        /// </summary>
        /// <returns></returns>
        public int GetStartDepth()
        {
            var startDepth = Items.Min(i => i.Z);

            return startDepth;
        }


        public int GetDepth()
        {
            var layerDepth = Items.Max(i => i.Z + i.Depth);

            return layerDepth - GetStartDepth();
        }

        public int CompareTo(PackedLayer other)
        {
            var result = other.GetFootprint().CompareTo(this.GetFootprint());
            if(result == 0)
            {
                result = other.GetDepth().CompareTo(this.GetDepth());
            }

            return result;
        }
    }
}
