using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class WastedFridgeItemTests
    {
        Category _category;

        [SetUp]
        public void BaseSetUp()
        {
            _category = new Category("Warzywa");
        }

        [Test]
        public void FridgeItem_Waste_ShouldSetIsWastedAndWastedAt()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            fridgeItem.WasteFridgeItem("expired");

            ClassicAssert.AreEqual(true, fridgeItem.IsWasted);
            ClassicAssert.IsNotNull(fridgeItem.WastedAt);
            ClassicAssert.AreEqual("expired", fridgeItem.WasteReason);
            ClassicAssert.AreEqual(false, fridgeItem.IsConsumed);
        }

        [Test]
        public void FridgeItem_Waste_WithoutReason_ShouldSetIsWasted()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(50.0f, Unit.Grams);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            fridgeItem.WasteFridgeItem();

            ClassicAssert.AreEqual(true, fridgeItem.IsWasted);
            ClassicAssert.IsNotNull(fridgeItem.WastedAt);
            ClassicAssert.IsNull(fridgeItem.WasteReason);
        }

        [Test]
        public void FridgeItem_WasteAlreadyWasted_ShouldThrowException()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            fridgeItem.WasteFridgeItem("spoiled");

            Assert.Throws(typeof(DomainException), () => fridgeItem.WasteFridgeItem("spoiled again"));
        }

        [Test]
        public void FridgeItem_WasteConsumedItem_ShouldThrowException()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            var consumeAmount = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(consumeAmount);

            ClassicAssert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.WasteFridgeItem("test"));
        }

        [Test]
        public void FridgeItem_ConsumeWastedItem_ShouldThrowException()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            fridgeItem.WasteFridgeItem("expired");

            var consumeAmount = new AmountValue(50.0f, Unit.Mililiter);
            Assert.Throws(typeof(DomainException), () => fridgeItem.ConsumeFridgeItem(consumeAmount));
        }

        [Test]
        public void FridgeItem_UpdateNoteOnWastedItem_ShouldThrowException()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            fridgeItem.WasteFridgeItem("expired");

            Assert.Throws(typeof(DomainException), () => fridgeItem.UpdateFridgeItemNote("new note"));
        }

        [Test]
        public void FridgeItem_WastedAt_ShouldBeRecentTimestamp()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            var before = DateTime.UtcNow;
            fridgeItem.WasteFridgeItem("expired");
            var after = DateTime.UtcNow;

            ClassicAssert.IsTrue(fridgeItem.WastedAt >= before);
            ClassicAssert.IsTrue(fridgeItem.WastedAt <= after);
        }
    }
}
