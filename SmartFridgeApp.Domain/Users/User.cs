using SmartFridgeApp.Domain.SeedWork;
using System;

namespace SmartFridgeApp.Domain.Users
{
    public class User :Entity
    {
        public Guid Id { get; }
        public string Name { get; }
        public DateTime CreatedAt { get; }
        public Guid FridgeId { get; }

    }
}
