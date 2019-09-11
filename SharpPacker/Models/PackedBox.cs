using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Models
{
    public class PackedBox
    {
        public Box box;
        public IEnumerable<PackedItem> packedItems;
    }
}
