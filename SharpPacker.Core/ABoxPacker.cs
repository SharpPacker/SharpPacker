using SharpPacker.Core.DataTypes;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Core
{
    public abstract class ABoxPacker<TOptions> : APacker<BoxPackerRequest, BoxPackerResult, TOptions>
    {
    }
}
