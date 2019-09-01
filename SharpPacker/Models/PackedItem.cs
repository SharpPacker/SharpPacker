namespace SharpPacker.Models
{
    public class PackedItem
    {
        public Item Item;

        public PackedItem()
        {
        }

        public PackedItem(Item item,
                int x,
                int y,
                int z,
                int width,
                int length,
                int depth
            )
        {
            this.Item = item;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Width = width;
            this.Length = length;
            this.Depth = depth;
        }

        public int Depth { get; set; }
        public int Length { get; set; }
        public float Volume => ((float)Width * Length * Depth);
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Weight => Item.Weight;

        public static PackedItem FromOrientatedItem(OrientatedItem oi, int x, int y, int z)
        {
            return new PackedItem()
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

        public OrientatedItem ToOrientatedItem()
        {
            var result = new OrientatedItem()
            {
                Item = this.Item,
                Width = this.Width,
                Length = this.Length,
                Depth = this.Depth,
            };

            return result;
        }

        public override string ToString()
        {
            return $"PackedItem4d {Item?.Description} [w{Width}, l{Length}, d{Depth}] (x{X}, y{Y}, z{Z})";
        }
    }
}
