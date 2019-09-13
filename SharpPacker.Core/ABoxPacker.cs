using SharpPacker.Core.DataTypes;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Core
{
    public abstract class ABoxPacker<TOptions> : IDisposable
    {
        public BoxPackerStepNotification stepNotify;

        public abstract void Init(TOptions options);

        public abstract BoxPackerResult PackBoxes(IEnumerable<BoxType> boxes, IEnumerable<Item> items);

        protected abstract void Dispose(bool disposing);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
