namespace SmartFridgeApp.Domain.SeedWork.Exceptions
{
    public class FridgeItemNotExistException : DomainException
    {
        public FridgeItemNotExistException() : base("")
        {

        }

        public FridgeItemNotExistException(string details) : base("DomainException", details)
        {
            
        }
    }
}
