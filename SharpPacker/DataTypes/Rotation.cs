using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.DataTypes
{
    public enum Rotation : byte
    {
        S123_to_S123 = 0b0000_0000, // DEFAULT
        S123_to_S132 = 0b0000_0001,

        S123_to_S213 = 0b0000_0010,
        S123_to_S231 = 0b0000_0100,

        S123_to_S312 = 0b0000_1000,
        S123_to_S321 = 0b0001_0000,
    }
}
