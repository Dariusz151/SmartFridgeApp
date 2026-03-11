using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Events;
using SmartFridgeApp.Core.Domain.Services;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.Domain;
using System;
using System.Collections.Generic;

namespace SmartFridgeApp.Core.Domain.Entities
{
    public class Fridge : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Desc { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public int WasteScore { get; private set; } = 1000;

        // ── Members ──
        private readonly List<FridgeMember> _members = [];
        public IReadOnlyCollection<FridgeMember> Members => _members.AsReadOnly();

        // ── Inventory tracking (shopping reminder) ──
        public int ActiveItemCount { get; private set; }
        public double AverageItemCount { get; private set; }
        public int InventorySampleCount { get; private set; }

        private const double SmoothingFactor = 0.15;
        private const double ShoppingThreshold = 0.5;
        private const int MinSamplesForAlert = 5;

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
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new FridgeCreatedEvent(this));
        }

        public void AddMember(FridgeMember member)
        {
            _members.Add(member);
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

        public void RecordItemConsumed(IFridgeScoringPolicy policy)
            => WasteScore += policy.CalculateConsumeReward();

        public void RecordItemWasted(IFridgeScoringPolicy policy)
            => WasteScore += policy.CalculateWastePenalty();

        public void RecordItemExpired(IFridgeScoringPolicy policy)
            => WasteScore += policy.CalculateExpiredItemPenalty();

        // ── Inventory tracking ──

        public void RecordItemAdded()
        {
            ActiveItemCount++;
            UpdateAverage();
        }

        public void RecordItemRemoved()
        {
            if (ActiveItemCount > 0)
                ActiveItemCount--;

            UpdateAverage();

            if (IsShoppingNeeded())
                AddDomainEvent(new ShoppingNeededDomainEvent(Id, ActiveItemCount, AverageItemCount));
        }

        public bool IsShoppingNeeded()
            => InventorySampleCount >= MinSamplesForAlert
               && AverageItemCount > 0
               && ActiveItemCount < AverageItemCount * ShoppingThreshold;

        private void UpdateAverage()
        {
            InventorySampleCount++;

            AverageItemCount = InventorySampleCount == 1
                ? ActiveItemCount
                : SmoothingFactor * ActiveItemCount + (1 - SmoothingFactor) * AverageItemCount;
        }
    }
}
