namespace SmartFridgeApp.Domain.SeedWork.Exceptions
{
    public class UserNotExistException : DomainException
    {
        public UserNotExistException() : base("")
        {

        }

        public UserNotExistException(string details) : base("DomainException", details)
        {

        }
    }
}
