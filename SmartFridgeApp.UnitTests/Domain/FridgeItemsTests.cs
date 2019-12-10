using NUnit.Framework;
using SmartFridgeApp.Domain.FridgeItems;
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

    }
}
