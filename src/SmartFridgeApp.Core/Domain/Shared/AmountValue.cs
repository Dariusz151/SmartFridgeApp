using System;
using Newtonsoft.Json;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.Core.Domain.Shared
{
    public class AmountValue
    {
        private const float TOLERANCE = 0.001f;

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
                throw new AmountValueException("Error while creating AmountValue","Value must be grater than 0");
            }
            this.Value = value;
            this.Unit = unit;
        }

        public void DecreaseAmount(AmountValue amountValue)
        {
            if (amountValue.Value > this.Value)
                this.Value = 0.0f;
            else
                this.Value = this.Value - amountValue.Value;
        }

        public void ResetAmount()
        {
            this.Value = 0.0f;
        }

        public int CompareTo(AmountValue obj)
        {
            if (obj.Unit.CompareTo(this.Unit) != 0)
            {
                throw new AmountValueException("Error comparing AmountValues", "AmountValues have other units. Can't compare them!");
            }

            if (Math.Abs(Value - obj.Value) < TOLERANCE)
                return 0;

            if (Value > obj.Value)
                return 1;

            return -1;
        }
    }
}
