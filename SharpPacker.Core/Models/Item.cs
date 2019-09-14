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

        public override bool Equals(object other)
        {
            return Equals(other as Item);
        }

        public bool Equals(Item other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;

            return String.Equals(this.Name, other.Name)
                && this.PhysicallyEquals(other);
        }

        public bool PhysicallyEquals(Item other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;

            if (!this.Dimensions.Equals(other.Dimensions)) return false;
            if (!this.Weight.Equals(other.Weight)) return false;

            if (this.AllowedRotations.Length != other.AllowedRotations.Length)
            {
                return false;
            }

            var otherRotations = other.AllowedRotations.Clone() as Rotation[];
            var thisRotations = this.AllowedRotations.Clone() as Rotation[];

            Array.Sort(otherRotations);
            Array.Sort(thisRotations);

            for(var i = 0; i < thisRotations.Length; i++)
            {
                if (!otherRotations[i].Equals(thisRotations[i]))
                {
                    return false;
                }
            }

            return true;

        }

    }
} 
