using SharpPacker.Core.DataTypes;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Core
{
    public abstract class APalletPacker
    {
        public PalletPackerStepNotification nolify;
        public abstract PalletPackerResult PackPallet(Pallet pallet, IEnumerable<Item> items);
    }
}
