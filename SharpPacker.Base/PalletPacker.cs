using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base
{

    public class PalletPacker : IPalletPacker
    {
        private IPalletPacker Strategy { get; set; }

        public PalletPacker(IPalletPacker strategy)
        {
            Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public PalletPackerResult Pack(PalletPackerRequest request)
        {
            return Strategy.Pack(request);
        }

        public void Dispose()
        {
            Strategy?.Dispose();
        }
    }
}
