using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Models
{
    public class PalletPackerResult
    {
        public PalletType Pallet { get; set; }
        public IEnumerable<PackedItem> PackedItems { get; set; }
    }
}
