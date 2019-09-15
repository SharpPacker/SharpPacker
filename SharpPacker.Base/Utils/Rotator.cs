using SharpPacker.Core.DataTypes;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.Utils
{
    public static class Rotator
    {
        public static Dimensions Rotate(Dimensions d, Rotation r)
        {
            switch (r)
            {
                case Rotation.XYZ_to_XYZ:
                    return new Dimensions(d.sizeX, d.sizeY, d.sizeZ);

                case Rotation.XYZ_to_XZY:
                    return new Dimensions(d.sizeX, d.sizeZ, d.sizeY);

                case Rotation.XYZ_to_YXZ:
                    return new Dimensions(d.sizeY, d.sizeX, d.sizeZ);

                case Rotation.XYZ_to_YZX:
                    return new Dimensions(d.sizeY, d.sizeZ, d.sizeX);

                case Rotation.XYZ_to_ZXY:
                    return new Dimensions(d.sizeZ, d.sizeX, d.sizeY);

                case Rotation.XYZ_to_ZYX:
                    return new Dimensions(d.sizeZ, d.sizeY, d.sizeX);
            }
            throw new ArgumentException($"Unknown type of Rotation: {r}");
        }
    }
}
