using BoxPackerClone.Models;

namespace BoxPackerClone.Tests
{
    internal static class Factory
    {
        public static Box CreateBox(string reference,
            int outerWidth,
            int outerLength,
            int outerDepth,
            int emptyWeight,
            int innerWidth,
            int innerLength,
            int innerDepth,
            int maxWeight)
        {
            return new Box()
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

        public static Item CreateItem(string description,
                                        int width,
                                        int length,
                                        int depth,
                                        int weight,
                                        bool keepFlat)
        {
            return new Item()
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
