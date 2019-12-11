using NUnit.Framework;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

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
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(name, desc, amountValue, userId);
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
        public void UpdateFridgeItemDetailsShouldHaveNewValues()
        {
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            var fridgeItem = new FridgeItem(name, desc, amountValue, userId);

            string nameUpdated = "updatedName";
            string descUpdated = "updatedDesc";

            fridgeItem.UpdateFridgeItemDetails(nameUpdated, descUpdated);
            Assert.AreEqual(fridgeItem.Name, nameUpdated);
            Assert.AreEqual(fridgeItem.Desc, descUpdated);
        }

        [Test]
        public void SetFridgeItemDetailsNameShouldntBeEmpty()
        {
            string name = String.Empty;
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(15.3f, Unit.Grams);

            FridgeItem fridgeItem;
            
            Assert.Throws(typeof(DomainException), () => fridgeItem = new FridgeItem(name, desc, amountValue, userId));
        }

        [Test]
        public void ConsumeFridgeItemWithGreaterAmountValueShouldSetIsConsumed()
        {
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(name, desc, amountValue, userId);

            AmountValue amountValToConsume = new AmountValue(110.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);
            
            Assert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void ConsumeFridgeItemWithLessAmountValueShouldntSetIsConsumed()
        {
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(name, desc, amountValue, userId);

            AmountValue amountValToConsume = new AmountValue(90.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(false, fridgeItem.IsConsumed);
            Assert.AreEqual(10.0f, fridgeItem.AmountValue.Value);
        }

        [Test]
        public void ConsumeFridgeItemWithSameAmountValueShouldSetIsConsumed()
        {
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(name, desc, amountValue, userId);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);

            fridgeItem.ConsumeFridgeItem(amountValToConsume);

            Assert.AreEqual(true, fridgeItem.IsConsumed);
        }

        [Test]
        public void ConsumeConsumedFridgeItemShouldThrowException()
        {
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(name, desc, amountValue, userId);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            Assert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.ConsumeFridgeItem(amountValToConsume));
        }

        [Test]
        public void UpdateConsumedFridgeItemShouldThrowException()
        {
            string name = "name";
            string desc = "desc";
            Guid userId = Guid.NewGuid();
            AmountValue amountValue = new AmountValue(100.0f, Unit.Mililiter);

            FridgeItem fridgeItem = new FridgeItem(name, desc, amountValue, userId);

            AmountValue amountValToConsume = new AmountValue(100.0f, Unit.Mililiter);
            fridgeItem.ConsumeFridgeItem(amountValToConsume); // first consume

            Assert.AreEqual(true, fridgeItem.IsConsumed);
            Assert.Throws(typeof(DomainException), () => fridgeItem.UpdateFridgeItemDetails("updated", "updated"));
        }
    }
}
