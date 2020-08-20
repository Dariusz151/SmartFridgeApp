using System;

namespace SmartFridgeApp.Domain.SeedWork.Exceptions
{
    public class DomainException : Exception
    {
        public string Details { get; }

        public DomainException(string message) : base(message)
        {

        }

        public DomainException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
