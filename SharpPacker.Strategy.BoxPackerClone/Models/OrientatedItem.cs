using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SharpPacker.Strategy.BoxPackerClone.Models
{
    public class OrientatedItem
    {
        private const double degree15inRadians = 0.261799;
        private static readonly ConcurrentDictionary<string, float> tippingPointCache = new ConcurrentDictionary<string, float>();

        public OrientatedItem()
        {
        }

        public OrientatedItem(Item item, int width, int length, int depth)
        {
            this.Item = item;
            this.Width = width;
            this.Length = length;
            this.Depth = depth;
        }

        public int Depth { get; set; }
        public Item Item { get; set; }

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

            tippingPointCache.AddOrUpdate(key, angle, (prevKey, prevAngle) => angle);

            return angle;
        }

        public bool IsStable()
        {
            var tp = GetTippingPoint();
            return tp > degree15inRadians;
        }

        public override string ToString()
        {
            return $"OrientatedItem {Item?.Description} [w{Width}, l{Length}, d{Depth}]";
        }
    }
}
