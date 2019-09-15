using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.DataTypes
{
    public enum Rotation : byte
    {
        XYZ_to_XYZ = 0b0000_0000,
        XYZ_to_XZY = 0b0000_0001,

        XYZ_to_YXZ = 0b0000_0010,
        XYZ_to_YZX = 0b0000_0100,

        XYZ_to_ZXY = 0b0000_1000,
        XYZ_to_ZYX = 0b0001_0000,
    }
}
