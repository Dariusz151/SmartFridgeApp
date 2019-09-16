using SmartFridgeApp.Types;
using System;

namespace SmartFridgeApp.Domain.Models
{
    public class User
    {
        public UserId Id { get; set; }
        public string Login { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
