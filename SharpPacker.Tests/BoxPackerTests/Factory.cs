using SharpPacker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Tests.BoxPackerTests
{
    static class Factory
    {
        public static Box4d CreateBox(string reference,
            int outerWidth,
            int outerLength,
            int outerDepth,
            int emptyWeight,
            int innerWidth,
            int innerLength,
            int innerDepth,
            int maxWeight)
        {
            return new Box4d()
            {
                Reference = reference,
                OuterWidth = outerWidth,
                OuterLength = outerLength,
                OuterDepth = outerDepth,
                EmptyWeight = emptyWeight,
                InnerWidth = innerWidth,
                InnerLength = innerLength,
                InnerDepth = innerDepth,
                MaxWeight = maxWeight,
            };
        }
    }
}
