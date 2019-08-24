namespace SharpPacker.Models
{
    internal class WorkingVolume4d : Box4d
    {
        private readonly int depth;
        private readonly int length;
        private readonly int maxWeight;
        private readonly int width;

        public WorkingVolume4d(int _width,
                                int _length,
                                int _depth,
                                int _maxWeight
                                )
        {
            width = _width;
            length = _length;
            depth = _depth;
            maxWeight = _maxWeight;
        }

        public override int EmptyWeight => 0;
        public override int InnerDepth => depth;
        public override int InnerLength => length;
        public override int InnerWidth => width;
        public override int MaxWeight => maxWeight;
        public override int OuterDepth => depth;
        public override int OuterLength => length;
        public override int OuterWidth => width;
        public override string Reference => "Working Volume";
    }
}
