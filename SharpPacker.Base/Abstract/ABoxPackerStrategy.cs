using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Base.Abstract
{
    public abstract class ABoxPackerStrategy<TOptions> : APackerStrategy<BoxPackerRequest, BoxPackerResult, TOptions>, IBoxPacker
                                                        where TOptions : class, new()
    {
    }
}
