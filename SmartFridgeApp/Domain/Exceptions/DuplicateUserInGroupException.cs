using System;

namespace SmartFridgeApp.Domain.Exceptions
{
    public class DuplicateUserInGroupException : Exception
    {
        public string Details { get; }

        public DuplicateUserInGroupException(string message) : base(message)
        {

        }

        public DuplicateUserInGroupException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
