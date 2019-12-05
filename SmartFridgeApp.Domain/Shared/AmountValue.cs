using SmartFridgeApp.Domain.SeedWork;
using System;

namespace SmartFridgeApp.Domain.Shared
{
    public class AmountValue
    {
        public float Value { get; private set; }
        public Unit Unit { get; private set; }

        public AmountValue(float value, Unit unit)
        {
            if (value <= 0)
            {
                throw new DomainException("Value of FridgeItem must be > 0");
            }
            this.Value = value;
            this.Unit = unit;
        }

        public void DecreaseAmount(AmountValue amountValue)
        {
            this.Value = this.Value - amountValue.Value;
        }

        public int CompareTo(AmountValue obj)
        {
            if (obj.Unit.CompareTo(this.Unit) != 0)
            {
                throw new DomainException("AmountValues have other units. Can't compare them!");
            }
            else
            {
                if (this.Value == obj.Value)
                {
                    return 0;
                }
                else
                {
                    if (this.Value > obj.Value)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
    }
}
