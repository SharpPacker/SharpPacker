using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Base
{
    public abstract class ABoxPacker<TOptions> : APacker<BoxPackerRequest, BoxPackerResult, TOptions>
    {
    }
}
