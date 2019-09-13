using SharpPacker.Core.DataTypes;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Core
{
    public abstract class APalletPacker<TOptions> : IDisposable
    {
        public PalletPackerStepNotification stepNotify;

        public abstract void Init(TOptions options);

        public abstract PalletPackerResult PackPallet(PalletType pallet, IEnumerable<Item> items);

        protected abstract void Dispose(bool disposing);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
