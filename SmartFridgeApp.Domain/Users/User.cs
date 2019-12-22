﻿using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Domain.Users.Events;

namespace SmartFridgeApp.Domain.Users
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

        public void AddFridgeItem(FridgeItem item)
        {
            _fridgeItems.Add(item);

            this.AddDomainEvent(new FridgeItemAdded(item));
        }

        public void RemoveFridgeItem(Guid fridgeItemId)
        {
            var fridgeItem = _fridgeItems.Single(fi => fi.Id == fridgeItemId);
            _fridgeItems.Remove(fridgeItem);

            this.AddDomainEvent(new FridgeItemRemoved(fridgeItem));
        }

        //public List<FridgeItemId> GetFridgeItemIds()
        //{
        //    return this._fridgeItems.Select(x => x.Id).ToList();
        //}
    }
}
