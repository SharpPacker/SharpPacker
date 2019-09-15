﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.DataTypes
{
    public struct Position
    {
        public Position(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public int X;
        public int Y;
        public int Z;

        public override string ToString()
        {
            return $"{{\"X\": {X}, \"Y\": {Y}, \"Z\": {Z}}}";
        }
    }
}
