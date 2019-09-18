using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Base
{
    public abstract class APalletPacker<TOptions> : APacker<PalletPackerRequest, PalletPackerResult, TOptions>
    {
    }
}
