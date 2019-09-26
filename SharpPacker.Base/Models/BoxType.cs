using SharpPacker.Base.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class BoxType
    {
        public BoxType(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Dimensions OuterDimensions { get; set; }
        public Dimensions InnerDimensions { get; set; }

        public uint EmptyWeight { get; set; } = 0;
        public uint MaxWeight { get; set; } = uint.MaxValue;

        public uint MinItemsCount { get; set; } = 0;
        public uint MaxItemsCount { get; set; } = uint.MaxValue;
    }
}
