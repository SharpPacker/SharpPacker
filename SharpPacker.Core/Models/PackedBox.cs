using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Models
{
    public class PackedBox
    {
        public BoxType Box { get; set; }
        public IEnumerable<PackedItem> PackedItems { get; set; }
    }
}
