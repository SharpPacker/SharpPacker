using SharpPacker.DataTypes;
using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Utils
{
    public static class Rotator
    {
        static Dimensions Rotate(Item i, Rotation r)
        {
            var d = i.dimensions;

            switch (r)
            {
                case Rotation.XYZ_to_XYZ:
                    return new Dimensions { sizeX = d.sizeX, sizeY = d.sizeY, sizeZ = d.sizeZ };

                case Rotation.XYZ_to_XZY:
                    return new Dimensions { sizeX = d.sizeX, sizeZ = d.sizeY, sizeY = d.sizeZ  };

                case Rotation.XYZ_to_YXZ:
                    return new Dimensions { sizeY = d.sizeX, sizeX = d.sizeY, sizeZ = d.sizeZ };

                case Rotation.XYZ_to_YZX:
                    return new Dimensions { sizeY = d.sizeX, sizeZ = d.sizeY, sizeX = d.sizeZ };

                case Rotation.XYZ_to_ZXY:
                    return new Dimensions { sizeY = d.sizeX, sizeZ = d.sizeY, sizeX = d.sizeZ };

                case Rotation.XYZ_to_ZYX:
                    return new Dimensions { sizeZ = d.sizeX, sizeY = d.sizeY, sizeX = d.sizeZ };
            }

            throw new ArgumentException($"Unknown type of Rotation: {r}");
        }
    }
}
