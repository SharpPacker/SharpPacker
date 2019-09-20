using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class BoxPackerRequest
    {
        public IEnumerable<BoxType> Boxes { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
