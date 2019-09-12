using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.DataTypes
{
    public struct Dimensions
    {
        public int sizeX;
        public int sizeY;
        public int sizeZ;

        public Dimensions Clone() => new Dimensions { sizeX = this.sizeX, sizeY = this.sizeY, sizeZ = this.sizeZ };
    }
}
