using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class WastedFridgeItemTests
    {
        Fridge _fridge;
        User _user;
        Category _category;

        [SetUp]
        public void BaseSetUp()
        {
            _fridge = new Fridge("lodówka", "Solika 5", "BEKO");
            _user = new User("Dario", "dario@mail.com");
            _fridge.AddUser(_user);
            _category = new Category("Warzywa");
        }

        [Test]
        public void FridgeItem_Waste_ShouldSetIsWastedAndWastedAt()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

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
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

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
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

            fridgeItem.WasteFridgeItem("spoiled");

            Assert.Throws(typeof(DomainException), () => fridgeItem.WasteFridgeItem("spoiled again"));
        }

        [Test]
        public void FridgeItem_WasteConsumedItem_ShouldThrowException()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

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
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

            fridgeItem.WasteFridgeItem("expired");

            var consumeAmount = new AmountValue(50.0f, Unit.Mililiter);
            Assert.Throws(typeof(DomainException), () => fridgeItem.ConsumeFridgeItem(consumeAmount));
        }

        [Test]
        public void FridgeItem_UpdateNoteOnWastedItem_ShouldThrowException()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

            fridgeItem.WasteFridgeItem("expired");

            Assert.Throws(typeof(DomainException), () => fridgeItem.UpdateFridgeItemNote("new note"));
        }

        [Test]
        public void User_WasteFridgeItem_ShouldDelegateToFridgeItem()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

            _user.AddFridgeItem(fridgeItem);
            _user.WasteFridgeItem(fridgeItem.Id, "spoiled");

            ClassicAssert.AreEqual(true, fridgeItem.IsWasted);
            ClassicAssert.AreEqual("spoiled", fridgeItem.WasteReason);
        }

        [Test]
        public void User_WasteFridgeItem_WithInvalidId_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => _user.WasteFridgeItem(999, "reason"));
        }

        [Test]
        public void FridgeItem_WastedAt_ShouldBeRecentTimestamp()
        {
            var foodProduct = new FoodProduct("Mleko", _category);
            var amountValue = new AmountValue(100.0f, Unit.Mililiter);
            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue);

            var before = DateTime.UtcNow;
            fridgeItem.WasteFridgeItem("expired");
            var after = DateTime.UtcNow;

            ClassicAssert.IsTrue(fridgeItem.WastedAt >= before);
            ClassicAssert.IsTrue(fridgeItem.WastedAt <= after);
        }
    }
}
