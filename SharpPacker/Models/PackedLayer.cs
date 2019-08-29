using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPacker.Models
{
    internal class PackedLayer : IComparable<PackedLayer>
    {
        public List<PackedItem4d> Items { get; set; } = new List<PackedItem4d>();

        public int CompareTo(PackedLayer other)
        {
            var result = other.GetFootprint().CompareTo(this.GetFootprint());
            if (result == 0)
            {
                result = other.GetDepth().CompareTo(this.GetDepth());
            }

            return result;
        }

        public int GetDepth()
        {
            var layerDepth = Items.Count > 0 ? Items.Max(i => i.Z + i.Depth) : 0;

            return layerDepth - GetStartDepth();
        }

        /// <summary>
        /// Calculate footprint area of this layer.
        /// </summary>
        /// <returns></returns>
        public int GetFootprint()
        {
            var layerWidth = Items.Count > 0 ? Items.Max(i => i.X + i.Width) : 0;
            var layerLength = Items.Count > 0 ? Items.Max(i => i.Y + i.Length) : 0;

            return layerWidth * layerLength;
        }

        /// <summary>
        /// Calculate start depth of this layer.
        /// </summary>
        /// <returns></returns>
        public int GetStartDepth()
        {
            var startDepth = Items.Count > 0 ? Items.Min(i => i.Z) : 0;

            return startDepth;
        }

        /// <summary>
        /// Add a packed item to this layer.
        /// </summary>
        /// <param name="packedItem"></param>
        public void Insert(PackedItem4d packedItem)
        {
            this.Items.Add(packedItem);
        }
    }
}
