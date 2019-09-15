using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class BoxPackerRequest
    {
        public List<BoxType> Boxes { get; } = new List<BoxType>();
        public List<Item> Items { get; } = new List<Item>();
    }
}
