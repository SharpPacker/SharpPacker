namespace SharpPacker.Models
{
    public class PackedItem4d
    {
        public Item4d Item;

        public PackedItem4d()
        {
        }

        public PackedItem4d(Item4d item,
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

        public static PackedItem4d FromOrientatedItem(OrientatedItem4d oi, int x, int y, int z)
        {
            return new PackedItem4d()
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

        public override string ToString()
        {
            return $"PackedItem4d {Item?.Description} [w{Width}, l{Length}, d{Depth}] (x{X}, y{Y}, z{Z})";
        }
    }
}
