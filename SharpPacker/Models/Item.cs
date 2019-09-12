using SharpPacker.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Models
{
    public class Item
    {
        public string name;

        public Dimensions dimensions;
        public Rotation[] allowedRotations;
        public Weight weight;
    }
} 
