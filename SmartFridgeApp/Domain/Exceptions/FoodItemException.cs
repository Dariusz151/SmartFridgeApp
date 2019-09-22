using System;

namespace SmartFridgeApp.Domain.Exceptions
{
    public class FoodItemException : Exception
    {
        public string Details { get; }

        public FoodItemException(string message) : base(message)
        {

        }

        public FoodItemException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
