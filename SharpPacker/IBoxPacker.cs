using SharpPacker.Models;
using System;
using System.Collections.Generic;

namespace SharpPacker.Interfaces
{
    public interface IBoxPacker
    {
        BoxPackerResult PackBox(IEnumerable<Box> boxes, IEnumerable<Item> items);
    }
}
