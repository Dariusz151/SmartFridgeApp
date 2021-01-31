using System;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Domain.Entities
{
    public class FridgeItem : Entity
    {
        public long Id { get; private set; }
        public short FoodProductId { get; set; }
        public virtual FoodProduct FoodProduct { get; set; }
        public string Note { get; private set; }
        public AmountValue AmountValue { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime EnteredAt { get; private set; }
        public bool IsConsumed { get; private set; } 
        public bool IsOutdated() => DateTime.Compare(ExpirationDate, DateTime.UtcNow) > 1;

        private FridgeItem()
        {

        }

        public FridgeItem(short foodProductId, string note, AmountValue amountValue)
        {
            //if (foodProduct is null)
            //    throw new DomainException("Food product is null", "InvalidFoodProduct");
            AmountValue = amountValue;
            IsConsumed = false;
            Note = note;
            FoodProductId = foodProductId;

            EnteredAt = DateTime.Now;
        }

        public void SetExpirationDate(DateTime datetime)
        {
            if (datetime.CompareTo(DateTime.Now.AddDays(-1)) < 0)
            {
                throw new DomainException("Cant set past expiration date!", "InvalidExpirationDate");
            }
            ExpirationDate = datetime;
        }

        public void ChangeFridgeItemAmount(AmountValue amountValue)
        {
            AmountValue = amountValue;
        }

        public void IncreaseFridgeItemAmount(AmountValue amountValue)
        {
            // handle unit - what if units are different?
            AmountValue = new AmountValue(AmountValue.Value + amountValue.Value, AmountValue.Unit);
        }

        public void UpdateFridgeItemNote(string note)
        {
            if (IsConsumed)
                throw new DomainException("This item is consumed! Cant update details.", "UpdateFridgeItemFailed");

            this.Note = note;
        }

        public void ConsumeFridgeItem(AmountValue amountValue)
        {
            if (IsConsumed)
                throw new DomainException("This item is consumed! Cant consume again.", "ConsumeFridgeItemFailed");

            if (this.AmountValue.CompareTo(amountValue) <= 0)
            {
                this.AmountValue.ResetAmount();
                IsConsumed = true;
            }
            else
            {
                IsConsumed = false;
                this.AmountValue.DecreaseAmount(amountValue);
            }
        }
    }
}
