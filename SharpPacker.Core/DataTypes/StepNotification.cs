﻿using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.DataTypes
{
    public delegate void StepNotification<TResult>(TResult stepResult, bool isLastStep);
}