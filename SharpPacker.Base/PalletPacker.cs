using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base
{
    public class PalletPacker : IPalletPackerStrategy
    {
        public IPalletPackerStrategy Strategy { get; set; }

        public PalletPacker() { }
        public PalletPacker(IPalletPackerStrategy strategy)
        {
            Strategy = strategy;
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
