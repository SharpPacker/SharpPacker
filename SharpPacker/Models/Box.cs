using System;

namespace SharpPacker.Models
{
    public class Box : IComparable<Box>
    {
        public virtual string Reference { get; set; }

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

        public virtual int InnerDepth { get; set; }
        public virtual int InnerLength { get; set; }
        public virtual int InnerWidth { get; set; }
        public float InnerVolume => ((float)InnerWidth * InnerLength * InnerDepth);

        public virtual int OuterDepth { get; set; }
        public virtual int OuterLength { get; set; }
        public virtual int OuterWidth { get; set; }
        public float OuterVolume => ((float)OuterWidth * OuterLength * OuterDepth);

        public int CompareTo(Box other)
        {
            throw new NotImplementedException();
        }

        override public string ToString()
        {
            return $"Box {Reference} [w{InnerWidth}, l{InnerLength}, d{InnerDepth}]";
        }
    }
}
