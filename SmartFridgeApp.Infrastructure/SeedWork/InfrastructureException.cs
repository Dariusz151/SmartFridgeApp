using System;

namespace SmartFridgeApp.Infrastructure
{
    public class InfrastructureException : Exception
    {
        public string Details { get; }

        public InfrastructureException(string message) : base(message)
        {

        }

        public InfrastructureException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
