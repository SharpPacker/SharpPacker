using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class BoxPackerRequest
    {
        public IEnumerable<BoxType> BoxTypes { get; set; }
        public IEnumerable<ItemsBundle> Bundles { get; set; }
    }
}
