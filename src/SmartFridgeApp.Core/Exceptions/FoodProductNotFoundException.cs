using System;

namespace SmartFridgeApp.Core.Exceptions
{
    public class FoodProductNotFoundException : Exception
    {
        public string Details { get; }

        public FoodProductNotFoundException(string message) : base(message)
        {

        }

        public FoodProductNotFoundException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
