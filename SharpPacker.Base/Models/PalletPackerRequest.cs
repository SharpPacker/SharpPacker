using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class PalletPackerRequest
    {
        public PalletType Pallet { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
