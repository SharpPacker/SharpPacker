using SharpPacker.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Models
{
    public class Item
    {
        public Item(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Dimensions Dimensions { get; set; }
        public Rotation[] AllowedRotations { get; set; }
        public Weight Weight { get; set; }
    }
} 
