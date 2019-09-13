﻿using SharpPacker.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Models
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

        public Weight EmptyWeight { get; set; } = 0;
        public Weight MaxWeight { get; set; } = int.MaxValue;

        public uint MinItemsCount { get; set; } = 0;
        public uint MaxItemsCount { get; set; } = uint.MaxValue;
    }
}
