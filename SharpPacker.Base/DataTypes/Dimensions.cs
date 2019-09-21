using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.DataTypes
{
    public struct Dimensions : IEquatable<Dimensions>
    {
        public Dimensions(uint sizeX, uint sizeY, uint sizeZ)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
        }

        public uint sizeX;
        public uint sizeY;
        public uint sizeZ;

        public bool Equals(Dimensions other) => (other.sizeX == this.sizeX && other.sizeY == this.sizeY && other.sizeZ == this.sizeZ);

        public override string ToString()
        {
            return $"{{\"sizeX\": {sizeX}, \"sizeY\": {sizeY}, \"sizeZ\": {sizeZ}}}";
        }
    }
}
