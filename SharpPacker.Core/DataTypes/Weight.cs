using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.DataTypes
{
    public struct Weight : IEquatable<Weight>, IEquatable<int>
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

        public bool Equals(Weight other) => (other._value == this._value);
        public bool Equals(int other) => (other == this._value);
    }
}
