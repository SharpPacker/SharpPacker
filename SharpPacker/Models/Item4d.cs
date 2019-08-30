using System;

namespace SharpPacker.Models
{
    public class Item4d : IComparable<Item4d>
    {
        public int Depth { get; set; }
        public string Description { get; set; }

        public bool KeepFlat { get; set; }
        public int Length { get; set; }
        public float Volume => ((float)Width * Length * Depth);
        public int Weight { get; set; }
        public int Width { get; set; }

        public int CompareTo(Item4d other)
        {
            var volumeDecider = this.Volume.CompareTo(other.Volume);
            if(volumeDecider != 0)
            {
                return -1 * volumeDecider;
            }

            var weightDecider = this.Weight - other.Weight;
            if (weightDecider != 0)
            {
                return -1 * weightDecider;
            }

            return -1 * other.Description.CompareTo(this.Description);
        }

        override public string ToString()
        {
            return $"Item {Description} [w{Width}, l{Length}, d{Depth}]";
        }
    }
}
