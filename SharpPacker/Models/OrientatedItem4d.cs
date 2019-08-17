﻿using System;
using System.Collections.Generic;

namespace SharpPacker.Models
{
    internal class OrientatedItem4d
    {
        private static readonly Dictionary<string, float> tippingPointCache = new Dictionary<string, float>();
        private const double degree15inRadians = 0.261799;

        public Item4d Item { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Width => Item.Width;
        public int Length => Item.Length;
        public int Depth => Item.Depth;

        public int SurfaceFootprint => (Width * Length);

        public bool IsStable()
        {
            var tp = GetTippingPoint();
            return tp > degree15inRadians;
        }

        private float GetTippingPoint()
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

        public override string ToString()
        {
            return $"{Width}|{Length}|{Depth}";
        }
    }
}