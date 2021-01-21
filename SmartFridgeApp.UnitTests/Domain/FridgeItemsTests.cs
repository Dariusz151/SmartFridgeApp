using NUnit.Framework;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FridgeItemsTests
    {
        Fridge _fridge;
        User _user;
        Category _category;
        [SetUp]
        public void BaseSetUp()
        {
            string fridgeName = "lodówa";
            string fridgeAddress = "Solika 5";
            string fridgeDesc = "BEKO";
            _fridge = new Fridge(fridgeName, fridgeAddress, fridgeDesc);
            _user = new User("Dario", "dario@mail.com");
            _fridge.AddUser(_user);
            _category = new Category("Warzywa");
        }

        [Test]
        public void FridgeItem_CreateNewShouldHaveDateTimeNow()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            var dateTime = DateTime.Now;

            Assert.AreEqual(dateTime.ToShortDateString(), fridgeItem.EnteredAt.ToShortDateString());
        }

        [Test]
        public void AmountValue_WithLessThanZero_ShouldThrowException()
        {
            AmountValue amountVal;
            Assert.Throws(typeof(AmountValueException), () => amountVal = new AmountValue(-10.0f, Unit.Grams));
        }

        [Test]
        public void FridgeItem_UpdateItemNote_ShouldHaveNewValue()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            string noteUpdated = "updatedDesc";

            fridgeItem.UpdateFridgeItemNote(noteUpdated);
            Assert.AreEqual(fridgeItem.Note, noteUpdated);
        }

        [Test]
        public void FridgeItem_ConsumeWithGreaterAmountValue_ShouldSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);

            string desc = "desc";
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(foodProduct, desc, amountValue);

            AmountValue amountValToConsume = new AmountValue(110.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void FridgeItem_ConsumeWithLessAmountValue_ShouldntSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            AmountValue amountValToConsume = new AmountValue(90.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(false, fridgeItem.IsConsumed);
            Assert.AreEqual(10.0f, fridgeItem.AmountValue.Value);
        }

        [Test]
        public void FridgeItem_ConsumeWithSameAmountValue_ShouldSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void FridgeItem_ConsumeConsumed_ShouldThrowException()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            Assert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.ConsumeFridgeItem(amountValToConsume));
        }

        [Test]
        public void FridgeItem_UpdateConsumed_ShouldThrowException()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            Assert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.UpdateFridgeItemNote("updated"));
        }
    }
}
