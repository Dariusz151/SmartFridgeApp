using System;

namespace SmartFridgeApp.Infrastructure.Validation
{
    public class CustomValidationException : Exception
    {
        public string Details { get; }

        public CustomValidationException(string message) : base(message)
        {

        }

        public CustomValidationException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
