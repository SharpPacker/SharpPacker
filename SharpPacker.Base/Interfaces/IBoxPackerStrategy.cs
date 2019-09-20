using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.Interfaces
{
    public interface IBoxPackerStrategy : IPackerStrategy<BoxPackerRequest, BoxPackerResult>
    {
    }
}
