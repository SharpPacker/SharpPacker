using SharpPacker.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Base.DataTypes
{
    public delegate void StepNotification<TResult>(TResult stepResult, bool isLastStep);
}
