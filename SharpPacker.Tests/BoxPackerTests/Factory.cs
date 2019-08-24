using SharpPacker.Models;

namespace SharpPacker.Tests.BoxPackerTests
{
    internal static class Factory
    {
        public static Box4d CreateBox(string reference,
            int outerWidth,
            int outerLength,
            int outerDepth,
            int emptyWeight,
            int innerWidth,
            int innerLength,
            int innerDepth,
            int maxWeight)
        {
            return new Box4d()
            {
                Reference = reference,
                OuterWidth = outerWidth,
                OuterLength = outerLength,
                OuterDepth = outerDepth,
                EmptyWeight = emptyWeight,
                InnerWidth = innerWidth,
                InnerLength = innerLength,
                InnerDepth = innerDepth,
                MaxWeight = maxWeight,
            };
        }

        public static Item4d CreateItem(string description,
                                        int width,
                                        int length,
                                        int depth,
                                        int weight,
                                        bool keepFlat)
        {
            return new Item4d()
            {
                Description = description,
                Width = width,
                Length = length,
                Depth = depth,
                Weight = weight,
                KeepFlat = keepFlat
            };
        }
    }
}
