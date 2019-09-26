using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Interfaces;
using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Base.Abstract
{
    public abstract class APalletPackerStrategy<TOptions> : APackerStrategy<PalletPackerRequest, PalletPackerResult, TOptions>, IPalletPacker
                                                            where TOptions : class, new()
    {
    }
}
