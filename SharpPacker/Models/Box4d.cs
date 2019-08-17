using System;

namespace SharpPacker.Models
{
    public class Box4d : IComparable<Box4d>
    {
        public virtual string Reference { get; set; }

        public virtual int OuterWidth { get; set; }
        public virtual int OuterLength { get; set; }
        public virtual int OuterDepth { get; set; }

        public virtual int InnerWidth { get; set; }
        public virtual int InnerLength { get; set; }
        public virtual int InnerDepth { get; set; }

        public virtual int EmptyWeight { get; set; }
        public virtual int MaxWeight { get; set; }

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

        public int InnerVolume => (InnerWidth * InnerLength * InnerDepth);
        public int OuterVolume => (OuterWidth * OuterLength * OuterDepth);

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
    }
}