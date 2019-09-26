using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base
{

    public class BoxPacker : IBoxPacker
    {
        private IBoxPacker Strategy { get; set; }

        public BoxPacker(IBoxPacker strategy)
        {
            Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public BoxPackerResult Pack(BoxPackerRequest request)
        {
            return Strategy.Pack(request);
        }

        public void Dispose()
        {
            Strategy?.Dispose();
        }
    }
}
