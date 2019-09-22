using System;

namespace SmartFridgeApp.Domain.Exceptions
{
    public class UserGroupException : Exception
    {
        public string Details { get; }

        public UserGroupException(string message) : base(message)
        {

        }

        public UserGroupException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}
