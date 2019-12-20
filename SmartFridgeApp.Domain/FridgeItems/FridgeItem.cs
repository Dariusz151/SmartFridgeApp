using SmartFridgeApp.Domain.FridgeItems.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using System;

namespace SmartFridgeApp.Domain.FridgeItems
{
    public class FridgeItem : Entity
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Desc { get; private set; }

        public AmountValue AmountValue { get; private set; }

        public DateTime ExpirationDate { get; set; }

        public Category Category { get; set; }

        public DateTime EnteredAt { get; private set; }

        public bool IsConsumed { get; private set; }

        public bool IsOutdated() => DateTime.Compare(ExpirationDate, DateTime.UtcNow) > 1;

        private FridgeItem()
        {

        }

        public FridgeItem(string name, string desc, AmountValue amountValue, Guid userId)
        {
            Id = Guid.NewGuid();
            EnteredAt = DateTime.Now;
            AmountValue = amountValue;
            IsConsumed = false;

            SetFridgeItemDetails(name, desc);
        }

        public void ChangeFridgeItemAmount(AmountValue amountValue)
        {
            AmountValue = amountValue;
        }

        public void UpdateFridgeItemDetails(string name, string desc)
        {
            if (IsConsumed)
                throw new DomainException("This item is consumed! Cant update details.");

            SetFridgeItemDetails(name, desc);
        }

        public void ConsumeFridgeItem(AmountValue amountValue)
        {
            if (IsConsumed)
                throw new DomainException("This item is consumed! Cant consume again.");

            if (this.AmountValue.CompareTo(amountValue) <= 0)
            {
                IsConsumed = true;
                this.AddDomainEvent(new FridgeItemConsumedEvent(this));
            }
            else
            {
                IsConsumed = false;
                this.AmountValue.DecreaseAmount(amountValue);
            }
        }

        private void SetFridgeItemDetails(string name, string desc)
        {
            if (name.Length < 1)
            {
                throw new DomainException("FridgeItem name can't be empty.");
            }
            Name = name;
            Desc = desc;
        }
    }
}
