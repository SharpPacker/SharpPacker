using SharpPacker.Base.DataTypes;
using SharpPacker.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpPacker.Base.Abstract
{
    public abstract class APackerStrategy<TRequest, TResult, TOptions> : IPacker<TRequest, TResult>, IDisposable
                                                                        where TOptions : class, new()
    {
        public abstract string StrategyName();
        public StepNotification<TResult> stepNotify;
        public readonly TOptions options;

        public APackerStrategy()
        {
            options = new TOptions();
        }

        public APackerStrategy(TOptions options)
        {
            this.options = options;
        }

        public abstract TResult Pack(TRequest request);

        protected abstract void Dispose(bool disposing);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
