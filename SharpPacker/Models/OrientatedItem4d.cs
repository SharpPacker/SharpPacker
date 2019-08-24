using System;
using System.Collections.Generic;

namespace SharpPacker.Models
{
    public class OrientatedItem4d
    {
        private const double degree15inRadians = 0.261799;
        private static readonly Dictionary<string, float> tippingPointCache = new Dictionary<string, float>();

        public OrientatedItem4d()
        {
        }

        public OrientatedItem4d(Item4d item, int width, int length, int depth)
        {
            this.Item = item;
            this.Width = width;
            this.Length = length;
            this.Depth = depth;
        }

        public int Depth { get; set; }
        public Item4d Item { get; set; }

        public int Length { get; set; }
        public int SurfaceFootprint => (Width * Length);
        public int Width { get; set; }

        public float GetTippingPoint()
        {
            var key = this.ToString();
            if (tippingPointCache.ContainsKey(key))
            {
                return tippingPointCache[key];
            }

            var tangens = (double)Math.Min(Width, Length) / (Depth != 0 ? Depth : 1);
            var angle = (float)Math.Atan(tangens);

            tippingPointCache.Add(key, angle);

            return angle;
        }

        public bool IsStable()
        {
            var tp = GetTippingPoint();
            return tp > degree15inRadians;
        }

        public override string ToString()
        {
            return $"{Width}|{Length}|{Depth}";
        }
    }
}
