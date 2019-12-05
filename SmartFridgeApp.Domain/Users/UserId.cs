using SmartFridgeApp.Domain.SeedWork;
using System;

namespace SmartFridgeApp.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value):base(value)
        {

        }
    }
}
