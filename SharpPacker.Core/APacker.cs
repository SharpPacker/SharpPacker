﻿using SharpPacker.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpPacker.Core
{
    public abstract class APacker<TRequest, TResult, TOptions> : IDisposable
    {
        public StepNotification<TResult> stepNotify;

        public abstract void Init(TOptions options);

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