﻿using System;

namespace SharpPacker.Models
{
    public class Box4d : IComparable<Box4d>
    {
        public virtual int EmptyWeight { get; set; }
        public virtual int InnerDepth { get; set; }
        public virtual int InnerLength { get; set; }
        public float InnerVolume => ((float)InnerWidth * InnerLength * InnerDepth);
        public virtual int InnerWidth { get; set; }
        public virtual int MaxWeight { get; set; }
        public virtual int OuterDepth { get; set; }
        public virtual int OuterLength { get; set; }
        public float OuterVolume => ((float)OuterWidth * OuterLength * OuterDepth);
        public virtual int OuterWidth { get; set; }
        public virtual string Reference { get; set; }

        public int WeightCapacity {
            get {
                if (MaxWeight <= 0)
                {
                    return 0;
                }
                else
                {
                    return MaxWeight - EmptyWeight;
                }
            }
        }

        public int CompareTo(Box4d other)
        {
            // try smallest box first
            if (this.InnerVolume > other.InnerVolume)
            {
                return 1;
            }
            if (this.InnerVolume < other.InnerVolume)
            {
                return -1;
            }

            // with smallest empty weight
            if (this.EmptyWeight < other.EmptyWeight)
            {
                return 1;
            }
            if (this.EmptyWeight > other.EmptyWeight)
            {
                return -1;
            }

            // maximum weight capacity as fallback decider
            return this.WeightCapacity.CompareTo(other.WeightCapacity);
        }

        override public string ToString()
        {
            return $"{Reference} [w{InnerWidth}, l{InnerLength}, d{InnerDepth}]";
        }
    }
}
