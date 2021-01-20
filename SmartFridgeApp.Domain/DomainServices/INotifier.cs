namespace SmartFridgeApp.Domain.DomainServices
{
    public interface INotifier
    {
        void SendMessage(string receiver, string msg);
    }
}
