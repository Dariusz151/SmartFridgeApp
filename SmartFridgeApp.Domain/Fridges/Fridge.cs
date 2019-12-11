using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Users;
using SmartFridgeApp.Domain.Users.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFridgeApp.Domain.Fridges
{
    public class Fridge : Entity, IAggregateRoot
    {
        public FridgeId Id { get; private set; }

        public string Name { get; private set; }

        public string Address { get; private set; }

        public string Desc { get; private set; }

        private List<User> _users;

        private Fridge()
        {
            
        }
        
        public Fridge(string name, string address, string desc)
        {
            Id = new FridgeId(Guid.NewGuid());
            Address = address;
            Name = name;
            Desc = desc;

            this._users = new List<User>();

            this.AddDomainEvent(new FridgeCreatedEvent(this));
        }

        public void AddUser(User user)
        {
            if (_users.Count(u => u.Id == user.Id) > 0)
            {
                throw new DomainException("Same user exists in this fridge.");
            }

            _users.Add(user);

            this.AddDomainEvent(new UserAddedEvent(user.Id));
        }

        public void RemoveUser(UserId userId)
        {
            try
            {
                var user = _users.Single(u => u.Id == userId);
                _users.Remove(user);

                this.AddDomainEvent(new UserRemovedEvent(user));
            }
            catch (InvalidOperationException)
            {
                throw new DomainException("Can't remove user that doesn't exist.");
            }
        }
        
        public List<FridgeItemId> GetFridgeItems(UserId userId)
        {
            var user = _users.Single(u => u.Id == userId);

            return user.GetFridgeItemIds();
        }

        public List<UserId> GetFridgeUsers()
        {
            var userIds = _users.Select(x => x.Id).ToList();
            return userIds;
        }

        //public void AddFridgeItem(FridgeItem item)
        //{
        //    // check if item is valid. domain exception if not

        //    _fridgeItems.Add(item);

        //    this.AddDomainEvent(new FridgeItemAddedEvent(item));
        //}
        
        //public void ConsumeFridgeItem(Guid fridgeItemId, AmountValue amountValue)
        //{
        //    _fridgeItems.Single(i => i.Id == fridgeItemId).ConsumeFridgeItem(amountValue);
        //}

        //public void EditFridgeDetails(string name, string desc, string address)
        //{
        //    Name = name;
        //    Desc = desc;
        //    Address = address;
        //}
    }
}
