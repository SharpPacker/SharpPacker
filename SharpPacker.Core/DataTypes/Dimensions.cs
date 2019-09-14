using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.DataTypes
{
    public struct Dimensions : IEquatable<Dimensions>
    {
        public Dimensions(int x, int y, int z)
        {
            sizeX = x;
            sizeY = y;
            sizeZ = z;
        }

        public int sizeX;
        public int sizeY;
        public int sizeZ;

        public bool Equals(Dimensions other) => (other.sizeX == this.sizeX && other.sizeY == this.sizeY && other.sizeZ == this.sizeZ);
    }
}
