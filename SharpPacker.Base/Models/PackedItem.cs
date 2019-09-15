using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class PackedItem
    {
        public PackedItem(Item item)
        {
            Item = item;
        }

        public Item Item { get; }

        public Dimensions Dimensions => Rotator.Rotate(Item.Dimensions, this.Rotation);

        public Position Position { get; set; }
        public Rotation Rotation { get; set; }
    }
}
