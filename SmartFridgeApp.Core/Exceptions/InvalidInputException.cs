using System;

namespace SmartFridgeApp.Core.Exceptions
{
    public class InvalidInputException : Exception
    {
        public string Details { get; }

        public InvalidInputException(string message) : base(message)
        {

        }

        public InvalidInputException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
