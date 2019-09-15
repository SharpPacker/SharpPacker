using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.DataTypes
{
    [Flags]
    public enum RotationFlags : byte
    {
        XYZ_to_XYZ = Rotation.XYZ_to_XYZ,
        XYZ_to_XZY = Rotation.XYZ_to_XZY,

        XYZ_to_YXZ = Rotation.XYZ_to_YXZ,
        XYZ_to_YZX = Rotation.XYZ_to_YZX,

        XYZ_to_ZXY = Rotation.XYZ_to_ZXY,
        XYZ_to_ZYX = Rotation.XYZ_to_ZYX,
    }
}
