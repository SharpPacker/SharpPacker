using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Models
{
    public class BoxPackerResult
    {
        public IEnumerable<PackedBox> packedBoxes;
        public IEnumerable<Item> unpackedItems;
    }
}
