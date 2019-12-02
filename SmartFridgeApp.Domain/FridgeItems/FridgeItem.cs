using SmartFridgeApp.Domain.FridgeItems.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using System;

namespace SmartFridgeApp.Domain.Fridges.FridgeItems
{
    public class FridgeItem : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public AmountValue AmountValue { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public Category Category { get; private set; }
        public DateTime EnteredAt { get; }
        public Guid UserId { get; }
        public bool _consumed;

        public bool IsOutdated() => DateTime.Compare(ExpirationDate, DateTime.UtcNow) > 1;

        public bool IsConsumed => _consumed;
        
        public FridgeItem(string name, string desc, AmountValue amountValue, Guid userId)
        {
            Id = Guid.NewGuid();
            EnteredAt = DateTime.Now;
            UserId = userId;
            _consumed = false;
            Name = name;
            Desc = desc;
            AmountValue = amountValue;
        }

        public void ChangeFridgeItemAmount(AmountValue amountValue)
        {
            AmountValue = amountValue;
        }

        public void UpdateFridgeItemDetails(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }

        public void SetCategory(Category category)
        {
            Category = category;
        }

        public void SetExpirationDate(DateTime expirationDateTime)
        {
            ExpirationDate = expirationDateTime;
        }

        public void ConsumeFridgeItem(AmountValue amountValue)
        {
            if (this.AmountValue.CompareTo(amountValue) > 0)
            {
                _consumed = true;
                this.AddDomainEvent(new FridgeItemConsumedEvent(this));
            }
            else
            {
                _consumed = false;
                this.AmountValue.DecreaseAmount(amountValue);
            }
        }
    }
}
