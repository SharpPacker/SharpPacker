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

        public Weight EmptyWeight { get; set; } = 0;
        public Weight MaxWeight { get; set; } = int.MaxValue;

        public uint MinItemsCount { get; set; } = 0;
        public uint MaxItemsCount { get; set; } = uint.MaxValue;
    }
}
