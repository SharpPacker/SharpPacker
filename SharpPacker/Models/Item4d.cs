﻿using System;

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

        public float Volume => ((float)Width * Length * Depth);

        public int CompareTo(Item4d other)
        {
            if (this.Volume > other.Volume)
            {
                return 1;
            }
            if (this.Volume < other.Volume)
            {
                return -1;
            }

            var weightDecider = this.Weight - other.Weight;
            if (weightDecider != 0)
            {
                return weightDecider;
            }

            return other.Description.CompareTo(.Description);
        }
    }
}