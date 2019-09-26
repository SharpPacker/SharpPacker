using System;

namespace SharpPacker.Strategy.BoxPackerClone.Models
{
    public class Item : IComparable<Item>
    {
        public int Depth { get; set; }
        public string Description { get; set; }

        public bool KeepFlat { get; set; }
        public int Length { get; set; }
        public float Volume => ((float)Width * Length * Depth);
        public int Weight { get; set; }
        public int Width { get; set; }

        public int CompareTo(Item other)
        {
            throw new NotImplementedException();
        }

        override public string ToString()
        {
            return $"Item {Description} [w{Width}, l{Length}, d{Depth}]";
        }
    }
}
