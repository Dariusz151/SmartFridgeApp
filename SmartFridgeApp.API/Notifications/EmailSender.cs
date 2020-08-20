using System;

namespace SmartFridgeApp.API.Notifications
{
    public class EmailSender : INotifier
    {
        public void SendMessage(string receiver, string msg)
        {
            Console.WriteLine($"Sending message to {receiver}: {msg}.");
        }
    }
}
