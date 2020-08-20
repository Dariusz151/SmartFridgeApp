using System;
using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Domain.Models.Fridges.Events;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.Models.Users.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.Domain.Models.Fridges
{
    public class Fridge : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Desc { get; private set; }
        public bool IsWelcomed { get; private set; }
        private List<User> _users;

        private Fridge()
        {
        }

        public Fridge(string name, string address, string desc)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Fridge should have a name.");
            Address = address;
            Name = name;
            Desc = desc;
            Id = Guid.NewGuid();
            IsWelcomed = false;

            _users = new List<User>();

            AddDomainEvent(new FridgeCreatedEvent(this));
        }

        public void MarkAsWelcomed()
        {
            IsWelcomed = true;
        }

        public void AddUser(User user)
        {
            if (_users.Count(u => u.Id == user.Id) > 0)
            {
                throw new DomainException("Same user exists in this fridge.");
            }

            _users.Add(user);

           // this.AddDomainEvent(new UserAddedEvent(user));
        }

        public void RemoveUser(Guid userId)
        {
            var user = GetFridgeUser(userId);
            _users.Remove(user);

            //this.AddDomainEvent(new UserRemovedEvent(user));
           
        }
        
        public List<Guid> GetFridgeUsers()
        {
            var userIds = _users.Select(x => x.Id).ToList();
            return userIds;
        }

        public User GetFridgeUser(Guid userId)
        {
            try
            {
                return _users.Single(u => u.Id == userId);
            }
            catch
            {
                throw new UserNotExistException("This user does not exist.");
            }
        }

        public void ChangeFridgeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Fridge should have a name.");
            Name = name;

            //this.AddDomainEvent(new FridgeUpdatedEvent(this));
        }

        public void ChangeFridgeDesc(string desc)
        {
            if (string.IsNullOrEmpty(desc))
                throw new DomainException("Fridge should have a description.");
            Desc = desc;

            //this.AddDomainEvent(new FridgeUpdatedEvent(this));
        }
    }
}
