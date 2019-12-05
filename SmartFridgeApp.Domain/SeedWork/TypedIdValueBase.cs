using System;

namespace SmartFridgeApp.Domain.SeedWork
{
    public abstract class TypedIdValueBase
    {
        public Guid Value { get; }

        protected TypedIdValueBase(Guid value)
        {
            Value = value;
        }
    }
}
