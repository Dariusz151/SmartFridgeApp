﻿using System;
using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Models.Users.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.SeedWork.Exceptions;
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

        private User()
        {

        }

        public User(string name, string email)
        {
            UpdateUserName(name);
            if (string.IsNullOrEmpty(email))
                throw new InvalidInputException("User email can't be empty.", "InvalidUserEmail");
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            _createdAt = DateTime.Now;

            this._fridgeItems = new List<FridgeItem>();
        }

        public void UpdateUserName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidInputException("User name can't be empty.", "InvalidUserName");
            Name = name;
        }
        
        public void AddFridgeItem(FridgeItem item)
        {
            try
            {
                var foodProducts = _fridgeItems.Select(x => x.FoodProduct.FoodProductId);
                if (foodProducts.Contains(item.FoodProduct.FoodProductId))
                {
                    _fridgeItems
                        .Where(x => x.FoodProduct.FoodProductId == item.FoodProduct.FoodProductId)
                        .First()
                        .IncreaseFridgeItemAmount(item.AmountValue);
                }
                else
                {
                    _fridgeItems.Add(item);
                }
            }
            catch
            {
                throw new AppException("Cant add FridgeItem for this user.", "AddFridgeItemFailed");
            }
        }

        public void RemoveFridgeItem(long fridgeItemId)
        {
            var fridgeItem = GetFridgeItem(fridgeItemId);
            _fridgeItems.Remove(fridgeItem);
            
            this.AddDomainEvent(new FridgeItemRemoved(fridgeItem));
        }
        
        public void ConsumeFridgeItem(long fridgeItemId, AmountValue amountValue)
        {
            var fridgeItem = GetFridgeItem(fridgeItemId);
            fridgeItem.ConsumeFridgeItem(amountValue);
        }
        
        private FridgeItem GetFridgeItem(long fridgeItemId)
        {
            try
            {
                return _fridgeItems.Single(fi => fi.Id == fridgeItemId);
            }
            catch
            {
                throw new InvalidInputException("FridgeItem with given id does not exist.", "InvalidFridgeItemId");
            }
        }
    }
}
