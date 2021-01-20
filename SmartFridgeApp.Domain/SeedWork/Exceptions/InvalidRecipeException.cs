using System;

namespace SmartFridgeApp.Domain.SeedWork.Exceptions
{
    public class InvalidRecipeException : Exception
    {
        public string Details { get; }

        public InvalidRecipeException(string message) : base(message)
        {

        }

        public InvalidRecipeException(string message, string details) : this(message)
        {
            Details = details;
        }
    }
}
