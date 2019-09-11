using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.DataTypes
{
    public struct Weight
    {
        private int _value;

        public static implicit operator Weight(int value)
        {
            return new Weight { _value = value };
        }

        public static implicit operator int(Weight w)
        {
            return w._value;
        }
    }
}
