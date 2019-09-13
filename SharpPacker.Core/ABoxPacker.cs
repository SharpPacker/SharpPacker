using SharpPacker.Core.DataTypes;
using SharpPacker.Core.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Core
{
    public abstract class ABoxPacker
    {
        public BoxPackerStepNotification Notify;
        public abstract BoxPackerResult PackBoxes(IEnumerable<Box> boxes, IEnumerable<Item> items);
    }
}
