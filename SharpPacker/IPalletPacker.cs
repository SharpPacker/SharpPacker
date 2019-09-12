using SharpPacker.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Interfaces
{
    public interface IPalletPacker
    {
        PalletPackerResult PackPallet(Pallet pallet, IEnumerable<Item> items);
    }
}
