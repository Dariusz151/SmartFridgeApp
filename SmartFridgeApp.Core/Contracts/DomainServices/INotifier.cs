namespace SmartFridgeApp.Core.Contracts.DomainServices
{
    public interface INotifier
    {
        void SendMessage(string receiver, string msg);
    }
}
