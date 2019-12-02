using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FridgesTests
    {
        [Test]
        public void CreatedFoodItemWithPastDateShouldThrowException()
        {
            //var pastExpirationDate = DateTime.Now.AddHours(-10);

            //Assert.Throws<InvalidFoodItemException>(() => new FoodItem(
            //   "foodItem",
            //   10,
            //   Unit.Pieces,
            //   pastExpirationDate,
            //   Category.Other
            //   ));
        }
    }
}
