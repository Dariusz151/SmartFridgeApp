using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFridgeApp.Core.Domain.Entities
{
    public class Fridge : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Desc { get; private set; }
        public int UsersCount { get; }
        private List<User> _users;

        private Fridge()
        {
        }

        public Fridge(string name, string address, string desc) : this()
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidInputException("Fridge should have a name.", "InvalidFridgeName");
            Id = Guid.NewGuid();
            Address = address;
            Name = name;
            Desc = desc;

            _users = new List<User>();
            UsersCount = _users.Count;

            AddDomainEvent(new FridgeCreatedEvent(this));
        }

        public void AddUser(User user)
        {
            if (user is null)
                throw new InvalidInputException("User object cant be null to add to fridge.", "InvalidUserObject");
            if (_users.Count(u => u.Id == user.Id) > 0)
            {
                throw new DomainException("Same user exists in this fridge.", "UserExist");
            }

            _users.Add(user);

            this.AddDomainEvent(new UserAddedEvent(user));
        }

        public void RemoveUser(Guid userId)
        {
            var user = GetFridgeUser(userId);
            _users.Remove(user);

            this.AddDomainEvent(new UserRemovedEvent(user));
        }
        
        public List<Guid> GetFridgeUsers()
        {
            try
            {
                return _users.Select(x => x.Id).ToList();
            }
            catch (ArgumentNullException)
            {
                throw new AppException("Can't get fridge users.","GetFridgeUsersFailed");
            }
        }

        public User GetFridgeUser(Guid userId)
        {
            try
            {
                return _users.Single(u => u.Id == userId);
            }
            catch
            {
                throw new InvalidInputException("This user does not belong to this fridge.", "UserNotBelongToFridge");
            }
        }

        public void ChangeFridgeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidInputException("Fridge should have a name.", "InvalidFridgeName");
            Name = name;
        }

        public void ChangeFridgeDesc(string desc)
        {
            if (string.IsNullOrEmpty(desc))
                throw new InvalidInputException("Fridge should have a description.", "InvalidFridgeDesc");
            Desc = desc;
        }
    }
}
