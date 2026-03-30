using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.Domain;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Domain.Entities
{
    public class Kitchen : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Desc { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<KitchenMember> _members = [];
        public IReadOnlyCollection<KitchenMember> Members => _members.AsReadOnly();

        private Kitchen() { }

        public Kitchen(string name, string address, string desc) : this()
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidInputException("Kitchen should have a name.", "InvalidKitchenName");
            Id = Guid.NewGuid();
            Address = address;
            Name = name;
            Desc = desc;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new KitchenCreatedEvent(this));
        }

        public void AddMember(KitchenMember member) => _members.Add(member);

        public void ChangeKitchenName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidInputException("Kitchen should have a name.", "InvalidKitchenName");
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeKitchenDesc(string desc)
        {
            if (string.IsNullOrEmpty(desc))
                throw new InvalidInputException("Kitchen should have a description.", "InvalidKitchenDesc");
            Desc = desc;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
