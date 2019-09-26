using SharpPacker.Base.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Models
{
    public class Item : IEquatable<Item>
    {
        public Item(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Dimensions Dimensions { get; set; }
        public RotationFlags AllowedRotations { get; set; }
        public uint Weight { get; set; }

        public bool PhysicallyEquals(Item other)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;

            if (!this.AllowedRotations.Equals(other.AllowedRotations)) return false;
            if (!this.Weight.Equals(other.Weight)) return false;
            if (!this.Dimensions.Equals(other.Dimensions)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1431379815;

            unchecked
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
                hashCode = hashCode * -1521134295 + EqualityComparer<Dimensions>.Default.GetHashCode(Dimensions);
                hashCode = hashCode * -1521134295 + EqualityComparer<RotationFlags>.Default.GetHashCode(AllowedRotations);
                hashCode = hashCode * -1521134295 + EqualityComparer<uint>.Default.GetHashCode(Weight);
            }
            return hashCode;
        }

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

        public static bool operator ==(Item item1, Item item2)
        {
            return EqualityComparer<Item>.Default.Equals(item1, item2);
        }

        public static bool operator !=(Item item1, Item item2)
        {
            return !(item1 == item2);
        }
    }
} 
