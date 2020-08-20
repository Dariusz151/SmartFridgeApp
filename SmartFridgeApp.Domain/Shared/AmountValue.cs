﻿using SmartFridgeApp.Domain.SeedWork;
using System;
using Newtonsoft.Json;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.Domain.Shared
{
    public class AmountValue
    {
        //TODO: If in database is incorrect Unit value -> crash (500 Internal server error)
        public float Value { get; private set; }
        public Unit Unit { get; private set; }

        private AmountValue()
        {

        }

        //[JsonConstructor]
        //public AmountValue(float value, Unit unit)
        //{
        //    this.Value = value;
        //    this.Unit = unit;
        //}


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
