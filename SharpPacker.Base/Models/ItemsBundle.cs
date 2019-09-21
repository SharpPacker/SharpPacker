using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class ItemsBundle
    {
        public Item Item {get; set;}
        public uint Quantity { get; set; } = 1;

        public ItemsBundle(Item item, uint quantity = 1)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
