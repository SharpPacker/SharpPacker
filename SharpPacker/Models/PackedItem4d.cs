namespace SharpPacker.Models
{
    public class PackedItem4d
    {
        internal static PackedItem4d FromOrientatedItem(OrientatedItem4d oi, int x, int y, int z)
        {
            return new PackedItem4d
            {
                Item = oi.Item,
                Width = oi.Width,
                Length = oi.Length,
                Depth = oi.Depth,
                X = x,
                Y = y,
                Z = z,
            };
        }

        public Item4d Item;

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Width { get; set; } // => Item.Width;
        public int Length { get; set; } // => Item.Length;
        public int Depth { get; set; }  // => Item.Depth;

        public int Weight => Item.Weight;

        public float Volume => Item.Volume;

        public static PackedItem4d FromOrienteditem(OrientatedItem4d oi, int x, int y, int z)
        {
            return new PackedItem4d()
            {
                Item = oi.Item,
                Width = oi.Item.Width,
                Length = oi.Item.Length,
                Depth = oi.Item.Depth,
                X = x,
                Y = y,
                Z = z,
            };
        }

        public OrientatedItem4d ToOrientatedItem()
        {
            var result = new OrientatedItem4d()
            {
                Item = this.Item,
                Width = this.Width,
                Length = this.Length,
                Depth = this.Depth,
            };

            return result;
        }
    }
}