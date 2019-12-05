using SmartFridgeApp.Domain.SeedWork;
using System;

namespace SmartFridgeApp.Domain.Fridges
{
    public class FridgeId : TypedIdValueBase
    {
        public FridgeId(Guid value) : base(value)
        {
        }
    }
}
