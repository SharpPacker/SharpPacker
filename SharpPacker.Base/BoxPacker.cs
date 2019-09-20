using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base
{
    public class BoxPacker : IBoxPackerStrategy
    {
        public IBoxPackerStrategy Strategy { get; set; }

        public BoxPacker() { }
        public BoxPacker(IBoxPackerStrategy strategy)
        {
            Strategy = strategy;
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
