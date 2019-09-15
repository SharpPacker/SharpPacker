﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class PalletPackerRequest
    {
        public PalletType Pallet { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
