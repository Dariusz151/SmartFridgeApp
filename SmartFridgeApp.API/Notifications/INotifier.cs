namespace SmartFridgeApp.API.Notifications
{
    public interface INotifier
    {
        void SendMessage(string receiver, string msg);
    }
}
