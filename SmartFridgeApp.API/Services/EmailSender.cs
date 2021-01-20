using SmartFridgeApp.Domain.DomainServices;
using System;

namespace SmartFridgeApp.API.Services
{
    public class EmailSender : INotifier
    {
        public void SendMessage(string receiver, string msg)
        {
            Console.WriteLine($"Sending message to {receiver}: {msg}.");
        }
    }
}
