using System;

namespace SmartFridgeApp.Core.Exceptions
{
    public class InvalidFridgeException : Exception
    {
        public string Details { get; }

        public InvalidFridgeException(string message) : base(message)
        {

        }

        public InvalidFridgeException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
