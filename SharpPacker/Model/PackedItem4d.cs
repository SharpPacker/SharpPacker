using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Model
{
    public class PackedItem4d
    {
        internal static PackedItem4d FromOrientatedItem(OrientatedItem4d oi, int x, int y, int z)
        {
            return new PackedItem4d
            {
                Item = oi.Item,
                X = x,
                Y = y,
                Z = z
            };
        }

        public Item4d Item;

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Width => Item.Width;
        public int Length => Item.Length;
        public int Depth => Item.Depth;

        public int Weight => Item.Weight;

        public int Volume => Item.Volume;
    }
}
