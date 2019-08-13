using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPacker.Exceptions
{
    public class ItemTooLargeException : Exception
    {
        public ItemTooLargeException() { }
        public ItemTooLargeException(String message) : base(message) { }
        public ItemTooLargeException(String message, Exception inner) : base(message, inner) { }
    }
}
