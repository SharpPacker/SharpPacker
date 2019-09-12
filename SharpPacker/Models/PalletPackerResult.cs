using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Models
{
    public class PalletPackerResult
    {
        public Pallet pallet;
        public IEnumerable<PackedItem> packedItems;
    }
}
