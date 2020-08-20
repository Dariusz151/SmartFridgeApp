namespace SmartFridgeApp.Domain.SeedWork.Exceptions
{
    public class InvalidFridgeIdException : DomainException
    {
        public InvalidFridgeIdException() : base("")
        {

        }

        public InvalidFridgeIdException(string details) : base("DomainException", details)
        {

        }
    }
}
