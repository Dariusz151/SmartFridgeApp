using System;

namespace SmartFridgeApp.Core.Exceptions
{
    public class AppException : Exception
    {
        public string Details { get; }

        public AppException(string message) : base(message)
        {

        }

        public AppException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
