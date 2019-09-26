using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Interfaces
{
    public interface IPacker<TRequest, TResult> : IDisposable
    {
        TResult Pack(TRequest request);
    }
}
