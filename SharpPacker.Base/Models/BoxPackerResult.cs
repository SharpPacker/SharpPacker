using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class BoxPackerResult
    {
        public IEnumerable<PackedBox> PackedBoxes { get; set; }
        public IEnumerable<ItemsBundle> UnpackedItems { get; set; }
    }
}
