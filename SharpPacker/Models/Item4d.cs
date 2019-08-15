using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Models
{
    public class Item4d : IComparable<Item4d>
    {
        public string Description { get; set; }

        public int Width { get; set; }
        public int Length { get; set; }
        public int Depth { get; set; }

        public int Weight { get; set; }

        public bool KeepFlat { get; set; }

        public int Volume => (Width * Length * Depth);

        public int CompareTo(Item4d other)
        {
            if(this.Volume > other.Volume)
            {
                return 1;
            }
            if (this.Volume < other.Volume)
            {
                return -1;
            }

            if(this.Weight > other.Weight)
            {
                return 1;
            }
            if (this.Weight < other.Weight)
            {
                return -1;
            }

            return this.Description.CompareTo(other.Description);
        }
    }
}
