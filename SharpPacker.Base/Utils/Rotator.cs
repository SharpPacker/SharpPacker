using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Utils
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

        public static Rotation GetRotation(Dimensions original, Dimensions rotated)
        {
            var XisX = (rotated.sizeX == original.sizeX);
            var XisY = (rotated.sizeX == original.sizeY);
            var XisZ = (rotated.sizeX == original.sizeZ);

            var YisX = XisY;
            var YisY = (rotated.sizeY == original.sizeY);
            var YisZ = (rotated.sizeY == original.sizeZ);

            var ZisX = XisZ;
            var ZisY = YisZ;
            var ZisZ = (rotated.sizeZ == original.sizeZ);

            var XYZ_to_XYZ = (XisX && YisY && ZisZ);
            var XYZ_to_ZXY = (ZisX && XisY && YisZ);
            var XYZ_to_YZX = (YisX && ZisY && XisZ);

            var XYZ_to_XZY = (XisX && ZisY && YisZ) || (XisX && YisZ);
            var XYZ_to_ZYX = (ZisX && YisY && XisZ) || (YisY && ZisX);
            var XYZ_to_YXZ = (YisX && XisY && ZisZ) || (ZisZ && XisY);

            if (XYZ_to_XYZ) return Rotation.XYZ_to_XYZ;
            if (XYZ_to_ZXY) return Rotation.XYZ_to_ZXY;
            if (XYZ_to_YZX) return Rotation.XYZ_to_YZX;

            if (XYZ_to_XZY) return Rotation.XYZ_to_XZY;
            if (XYZ_to_ZYX) return Rotation.XYZ_to_ZYX;
            if (XYZ_to_YXZ) return Rotation.XYZ_to_YXZ;

            throw new InvalidOperationException($"Unknown rotation from {original} to {rotated}");
        }
    }
}
