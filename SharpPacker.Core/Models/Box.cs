using SharpPacker.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Models
{
    public class Box
    {
        public string name;

        public Dimensions outerDimensions;
        public Dimensions innerDimensions;

        public Weight emptyWeight;
        public Weight maxWeight;

        public uint minItemsCount = uint.MinValue;
        public uint maxItemsCount = uint.MaxValue;
    }
}
