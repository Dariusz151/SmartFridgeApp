using System;

namespace SmartFridgeApp.Domain.Exceptions
{
    public class InvalidFoodItemException : Exception
    {
        public string Details { get; }

        public InvalidFoodItemException(string message) : base(message)
        {

        }

        public InvalidFoodItemException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
