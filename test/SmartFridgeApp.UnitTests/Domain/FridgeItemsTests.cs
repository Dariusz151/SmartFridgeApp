using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FridgeItemsTests
    {
        Category _category;

        [SetUp]
        public void BaseSetUp()
        {
            _category = new Category("Warzywa");
        }

        [Test]
        public void FridgeItem_CreateNewShouldHaveDateTimeNow()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);
            var dateTime = DateTime.Now;

            ClassicAssert.AreEqual(dateTime.ToShortDateString(), fridgeItem.EnteredAt.ToShortDateString());
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

            var fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);
            string noteUpdated = "updatedDesc";

            fridgeItem.UpdateFridgeItemNote(noteUpdated);
            ClassicAssert.AreEqual(fridgeItem.Note, noteUpdated);
        }

        [Test]
        public void FridgeItem_ConsumeWithGreaterAmountValue_ShouldSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);

            string desc = "desc";
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(foodProduct.FoodProductId, desc, amountValue, 1);

            AmountValue amountValToConsume = new AmountValue(110.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            ClassicAssert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void FridgeItem_ConsumeWithLessAmountValue_ShouldntSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);
            AmountValue amountValToConsume = new AmountValue(90.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            ClassicAssert.AreEqual(false, fridgeItem.IsConsumed);
            ClassicAssert.AreEqual(10.0f, fridgeItem.AmountValue.Value);
        }

        [Test]
        public void FridgeItem_ConsumeWithSameAmountValue_ShouldSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);
            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            ClassicAssert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void FridgeItem_ConsumeConsumed_ShouldThrowException()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            ClassicAssert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.ConsumeFridgeItem(amountValToConsume));
        }

        [Test]
        public void FridgeItem_UpdateConsumed_ShouldThrowException()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko", _category);
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct.FoodProductId, "desc", amountValue, 1);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            ClassicAssert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.UpdateFridgeItemNote("updated"));
        }
    }
}
