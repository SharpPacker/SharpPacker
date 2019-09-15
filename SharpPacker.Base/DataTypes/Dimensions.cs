using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.DataTypes
{
    public struct Dimensions : IEquatable<Dimensions>
    {
        public Dimensions(int sizeX, int sizeY, int sizeZ)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
        }

        public int sizeX;
        public int sizeY;
        public int sizeZ;

        public bool Equals(Dimensions other) => (other.sizeX == this.sizeX && other.sizeY == this.sizeY && other.sizeZ == this.sizeZ);

        public override string ToString()
        {
            return $"{{\"sizeX\": {sizeX}, \"sizeY\": {sizeY}, \"sizeZ\": {sizeZ}}}";
        }
    }
}
