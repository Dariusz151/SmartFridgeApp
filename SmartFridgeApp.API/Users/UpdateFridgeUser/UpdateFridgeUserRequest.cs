using System;

namespace SmartFridgeApp.API.Users.UpdateFridgeUser
{
    public class UpdateFridgeUserRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
