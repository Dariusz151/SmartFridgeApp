using System;

namespace SmartFridgeApp.Core.Exceptions
{

    public class AmountValueException : Exception
    {
        public string Details { get; }

        public AmountValueException(string message) : base(message)
        {

        }

        public AmountValueException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
