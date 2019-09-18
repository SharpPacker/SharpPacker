using SharpPacker.Base.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpPacker.Base
{
    public abstract class APacker<TRequest, TResult, TOptions> : IDisposable where TOptions : class, new()
    {
        public StepNotification<TResult> stepNotify;
        public readonly TOptions options = new TOptions();

        public abstract TResult Pack(TRequest request);
        public abstract Task<TResult> PackAsync(TRequest request, CancellationToken cancellationToken);

        protected abstract void Dispose(bool disposing);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
