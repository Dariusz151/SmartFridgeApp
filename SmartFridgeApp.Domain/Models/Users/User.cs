using System;
using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Models.Users.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.Domain.Models.Users
{
    public class User : Entity
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        private List<FridgeItem> _fridgeItems;

        private DateTime _createdAt;

        private bool _welcomeEmailWasSent;

        private User()
        {

        }

        public User(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            _createdAt = DateTime.Now;

            this._fridgeItems = new List<FridgeItem>();
            _welcomeEmailWasSent = false;
        }

        public void UpdateUser(string name)
        {
            if (!string.IsNullOrEmpty(name))
                Name = name;

            this.AddDomainEvent(new UserUpdatedEvent(this));
        }
        
        public void AddFridgeItem(FridgeItem item)
        {
            _fridgeItems.Add(item);

            this.AddDomainEvent(new FridgeItemAdded(item));
        }

        public void RemoveFridgeItem(Guid fridgeItemId)
        {
            var fridgeItem = GetFridgeItem(fridgeItemId);
            _fridgeItems.Remove(fridgeItem);

            this.AddDomainEvent(new FridgeItemRemoved(fridgeItem));
        }
        
        public void ConsumeFridgeItem(Guid fridgeItemId, AmountValue amountValue)
        {
            var fridgeItem = this.GetFridgeItem(fridgeItemId);
            fridgeItem.ConsumeFridgeItem(amountValue);

            this.AddDomainEvent(new FridgeItemConsumed(fridgeItem));
        }
        
        private FridgeItem GetFridgeItem(Guid fridgeItemId)
        {
            var fridgeItem = _fridgeItems.Single(fi => fi.Id == fridgeItemId);
            return fridgeItem;
        }

        //public List<FridgeItemId> GetFridgeItemIds()
        //{
        //    return this._fridgeItems.Select(x => x.Id).ToList();
        //}
    }
}
