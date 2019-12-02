using SmartFridgeApp.Domain.Fridges.Events;
using SmartFridgeApp.Domain.Fridges.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using SmartFridgeApp.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFridgeApp.Domain.Fridges
{
    public class Fridge : Entity, IAggregateRoot
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Desc { get; private set; }

        private List<FridgeItem> _fridgeItems;
        private List<User> _users;

        private Fridge(string name, string address, string desc)
        {
            Id = Guid.NewGuid();
            _fridgeItems = new List<FridgeItem>();
            _users = new List<User>();
            Name = name;
            Desc = desc;

            this.AddDomainEvent(new FridgeCreatedEvent(this));
        }

        public Guid CreateFridge(string name, string address, string desc)
        {
            new Fridge(name, address, desc);
            return this.Id;
        }

        public void AddFridgeItem(FridgeItem item)
        {
            // check if item is valid. domain exception if not

            _fridgeItems.Add(item);

            this.AddDomainEvent(new FridgeItemAddedEvent(item));
        }
        
        public void ConsumeFridgeItem(Guid fridgeItemId, AmountValue amountValue)
        {
            _fridgeItems.Single(i => i.Id == fridgeItemId).ConsumeFridgeItem(amountValue);
        }

        public void EditFridgeDetails(string name, string desc, string address)
        {
            Name = name;
            Desc = desc;
            Address = address;
        }
    }
}
