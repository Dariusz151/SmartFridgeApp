using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartFridgeApp.Core.Domain.Inventory;
using SmartFridgeApp.Core.Domain.Inventory.Events;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class KitchenInventoryTests
    {
        private KitchenInventory _inventory;

        [SetUp]
        public void BaseSetUp()
        {
            _inventory = new KitchenInventory { Id = Guid.NewGuid() };
        }

        [Test]
        public void StockItem_WithValidData_ShouldAddActiveItem()
        {
            var evt = _inventory.StockItem(1, 1, 500f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Milk");

            ClassicAssert.AreEqual(1, _inventory.ActiveItemCount);
            ClassicAssert.AreEqual(1, _inventory.ActiveItems.Count);
            ClassicAssert.IsTrue(_inventory.ActiveItems.ContainsKey(evt.ItemId));
        }

        [Test]
        public void StockItem_WithNegativeAmount_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() =>
                _inventory.StockItem(1, 1, -10f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Bad"));
        }

        [Test]
        public void StockItem_WithPastExpirationDate_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() =>
                _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(-5), "Expired"));
        }

        [Test]
        public void ConsumeItem_PartialAmount_ShouldDecreaseButKeepActive()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Mililiter, DateTime.UtcNow.AddDays(7), "Juice");

            var consumed = _inventory.ConsumeItem(stocked.ItemId, 1, 40f, Unit.Mililiter);

            ClassicAssert.IsFalse(consumed.IsFullyConsumed);
            ClassicAssert.AreEqual(40f, consumed.AmountConsumed);
            ClassicAssert.AreEqual(60f, _inventory.ActiveItems[stocked.ItemId].Amount);
            ClassicAssert.AreEqual(1, _inventory.ActiveItemCount);
        }

        [Test]
        public void ConsumeItem_FullAmount_ShouldRemoveFromActive()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Mililiter, DateTime.UtcNow.AddDays(7), "Juice");

            var consumed = _inventory.ConsumeItem(stocked.ItemId, 1, 100f, Unit.Mililiter);

            ClassicAssert.IsTrue(consumed.IsFullyConsumed);
            ClassicAssert.AreEqual(0, _inventory.ActiveItems.Count);
        }

        [Test]
        public void ConsumeItem_MoreThanAvailable_ShouldCapAndFullyConsume()
        {
            var stocked = _inventory.StockItem(1, 1, 50f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Cheese");

            var consumed = _inventory.ConsumeItem(stocked.ItemId, 1, 100f, Unit.Grams);

            ClassicAssert.IsTrue(consumed.IsFullyConsumed);
            ClassicAssert.AreEqual(50f, consumed.AmountConsumed);
        }

        [Test]
        public void ConsumeItem_NotFound_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() =>
                _inventory.ConsumeItem(Guid.NewGuid(), 1, 10f, Unit.Grams));
        }

        [Test]
        public void ConsumeItem_WrongMember_ShouldThrowException()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Item");

            Assert.Throws<DomainException>(() =>
                _inventory.ConsumeItem(stocked.ItemId, 999, 10f, Unit.Grams));
        }

        [Test]
        public void ConsumeItem_UnitMismatch_ShouldThrowException()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Item");

            Assert.Throws<DomainException>(() =>
                _inventory.ConsumeItem(stocked.ItemId, 1, 10f, Unit.Mililiter));
        }

        [Test]
        public void WasteItem_ShouldRemoveFromActiveAndDecrementScore()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Item");
            int scoreBefore = _inventory.WasteScore;

            _inventory.WasteItem(stocked.ItemId, 1, "Spoiled");

            ClassicAssert.AreEqual(0, _inventory.ActiveItems.Count);
            ClassicAssert.IsTrue(_inventory.WasteScore < scoreBefore);
        }

        [Test]
        public void WasteItem_NotFound_ShouldThrowException()
        {
            Assert.Throws<DomainException>(() =>
                _inventory.WasteItem(Guid.NewGuid(), 1, "Bad"));
        }

        [Test]
        public void RemoveItem_ShouldRemoveFromActive()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Item");

            _inventory.RemoveItem(stocked.ItemId, 1);

            ClassicAssert.AreEqual(0, _inventory.ActiveItems.Count);
        }

        [Test]
        public void ConsumeItem_ShouldIncreaseWasteScore()
        {
            var stocked = _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Item");
            int scoreBefore = _inventory.WasteScore;

            _inventory.ConsumeItem(stocked.ItemId, 1, 100f, Unit.Grams);

            ClassicAssert.IsTrue(_inventory.WasteScore > scoreBefore);
        }

        [Test]
        public void GetScoreRank_DefaultScore_ShouldBeResponsible()
        {
            ClassicAssert.AreEqual("Responsible", _inventory.GetScoreRank());
        }

        [Test]
        public void IsShoppingNeeded_NotEnoughSamples_ShouldBeFalse()
        {
            _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Item");

            ClassicAssert.IsFalse(_inventory.IsShoppingNeeded());
        }

        [Test]
        public void StockItem_WithLocation_ShouldStoreLocation()
        {
            var evt = _inventory.StockItem(1, 1, 500f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Frozen peas",
                StorageLocation.Freezer);

            ClassicAssert.AreEqual(StorageLocation.Freezer, evt.Location);
            ClassicAssert.AreEqual(StorageLocation.Freezer, _inventory.ActiveItems[evt.ItemId].Location);
        }

        [Test]
        public void StockItem_WithoutLocation_ShouldDefaultToFridge()
        {
            var evt = _inventory.StockItem(1, 1, 500f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Milk");

            ClassicAssert.AreEqual(StorageLocation.Fridge, evt.Location);
            ClassicAssert.AreEqual(StorageLocation.Fridge, _inventory.ActiveItems[evt.ItemId].Location);
        }

        [Test]
        public void StockItem_WithTags_ShouldStoreTags()
        {
            var tags = new List<ItemTag> { ItemTag.Organic, ItemTag.Vegan };
            var evt = _inventory.StockItem(1, 1, 200f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Tofu",
                StorageLocation.Fridge, tags);

            ClassicAssert.AreEqual(2, evt.Tags.Count);
            CollectionAssert.Contains(evt.Tags, ItemTag.Organic);
            CollectionAssert.Contains(evt.Tags, ItemTag.Vegan);
            ClassicAssert.AreEqual(2, _inventory.ActiveItems[evt.ItemId].Tags.Count);
        }

        [Test]
        public void StockItem_WithoutTags_ShouldHaveEmptyTags()
        {
            var evt = _inventory.StockItem(1, 1, 100f, Unit.Grams, DateTime.UtcNow.AddDays(7), "Butter");

            ClassicAssert.IsNotNull(evt.Tags);
            ClassicAssert.AreEqual(0, evt.Tags.Count);
        }
    }
}
