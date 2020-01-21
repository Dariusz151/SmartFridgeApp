using NUnit.Framework;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.FridgeItems;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FridgeItemsTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            //string fridgeName = "lodówa";
            //string fridgeAddress = "Solika 5";
            //string fridgeDesc = "BEKO";
            //_fridge = new Fridge(fridgeName, fridgeAddress, fridgeDesc);
        }

        [Test]
        public void CreateNewFridgeItemShouldHaveDateTimeNow()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            var dateTime = DateTime.Now;

            Assert.AreEqual(dateTime.ToShortDateString(), fridgeItem.EnteredAt.ToShortDateString());
        }

        [Test]
        public void AmountValueShouldntBeLessThanZero()
        {
            AmountValue amountVal;
            Assert.Throws(typeof(DomainException), () => amountVal = new AmountValue(-10.0f, Unit.Grams));
        }

        [Test]
        public void UpdateFridgeItemDescriptionShouldHaveNewValue()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            string descUpdated = "updatedDesc";

            fridgeItem.UpdateFridgeItemDescription(descUpdated);
            Assert.AreEqual(fridgeItem.Desc, descUpdated);
        }

        [Test]
        public void SetFridgeItemDetailsNameShouldntBeEmpty()
        {
            string desc = "desc";
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            FridgeItem fridgeItem;

            Assert.Throws(typeof(DomainException), () => fridgeItem = new FridgeItem(new FoodProduct(String.Empty), desc, amountValue));
        }

        [Test]
        public void ConsumeFridgeItemWithGreaterAmountValueShouldSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");

            string desc = "desc";
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(foodProduct, desc, amountValue);

            AmountValue amountValToConsume = new AmountValue(110.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void ConsumeFridgeItemWithLessAmountValueShouldntSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            AmountValue amountValToConsume = new AmountValue(90.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(false, fridgeItem.IsConsumed);
            Assert.AreEqual(10.0f, fridgeItem.AmountValue.Value);
        }

        [Test]
        public void ConsumeFridgeItemWithSameAmountValueShouldSetIsConsumed()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);
            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void ConsumeConsumedFridgeItemShouldThrowException()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            Assert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.ConsumeFridgeItem(amountValToConsume));
        }

        [Test]
        public void UpdateConsumedFridgeItemShouldThrowException()
        {
            FoodProduct foodProduct = new FoodProduct("Mleko");
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);
            FridgeItem fridgeItem = new FridgeItem(foodProduct, "desc", amountValue);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            Assert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.UpdateFridgeItemDescription("updated"));
        }
    }
}
