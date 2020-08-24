using System;
using Newtonsoft.Json;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.Domain.Shared
{
    public class AmountValue
    {
        private const float TOLERANCE = 0.001f;

        // this setters should be private (domain driven design violate) -
        // but don't know how to serialize to xml (maybe create Dto?)
        public float Value { get; set; }
        public Unit Unit { get; set; }
        
        private AmountValue()
        {

        }
        
        public AmountValue(float value)
            :this(value, Unit.NotAssigned)
        {

        }

        [JsonConstructor]
        public AmountValue(float value, Unit unit)
        {
            if (value <= 0)
            {
                throw new DomainException("Error while creating AmountValue","Value of FridgeItem must be > 0");
            }
            this.Value = value;
            this.Unit = unit;
        }

        public void DecreaseAmount(AmountValue amountValue)
        {
            this.Value = this.Value - amountValue.Value;
        }

        public void ResetAmount()
        {
            this.Value = 0;
        }

        public int CompareTo(AmountValue obj)
        {
            if (obj.Unit.CompareTo(this.Unit) != 0)
            {
                throw new DomainException("Error comparing AmountValues", "AmountValues have other units. Can't compare them!");
            }

            if (Math.Abs(Value - obj.Value) < TOLERANCE)
                return 0;

            if (Value > obj.Value)
                return 1;

            return -1;
        }
    }
}
