using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class PalletType
    {
        public PalletType(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Dimensions Dimensions { get; set; }

        public uint EmptyWeight { get; set; } = 0;
        public uint MaxWeight { get; set; } = int.MaxValue;

        public uint MinItemsCount { get; set; } = 0;
        public uint MaxItemsCount { get; set; } = uint.MaxValue;
    }
}
