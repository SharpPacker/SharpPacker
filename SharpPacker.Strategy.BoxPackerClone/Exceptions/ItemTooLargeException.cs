using System;

namespace SharpPacker.Strategy.BoxPackerClone.Exceptions
{
    public class ItemTooLargeException : Exception
    {
        public ItemTooLargeException()
        {
        }

        public ItemTooLargeException(String message) : base(message)
        {
        }

        public ItemTooLargeException(String message, Exception inner) : base(message, inner)
        {
        }
    }
}
