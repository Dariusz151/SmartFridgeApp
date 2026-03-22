using System;

namespace SmartFridgeApp.Core.Exceptions
{
    public class InvalidKitchenException : Exception
    {
        public string Details { get; }

        public InvalidKitchenException(string message) : base(message)
        {

        }

        public InvalidKitchenException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
