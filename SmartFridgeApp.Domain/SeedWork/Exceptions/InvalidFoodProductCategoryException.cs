using System;

namespace SmartFridgeApp.Domain.SeedWork.Exceptions
{
    public class InvalidFoodProductCategoryException : Exception
    {
        public string Details { get; }

        public InvalidFoodProductCategoryException(string message) : base(message)
        {

        }

        public InvalidFoodProductCategoryException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
