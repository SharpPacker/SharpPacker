﻿using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Core.DataTypes
{
    public delegate void PalletPackerStepNotification(PalletPackerResult stepResult, bool isLastStep);
}