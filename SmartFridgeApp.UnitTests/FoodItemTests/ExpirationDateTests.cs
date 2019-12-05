using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartFridgeApp.UnitTests.FoodItemTests
{
    [TestFixture]
    public class ExpirationDateTests
    {
        //[Test]
        //public void CreatedFoodItemWithPastDateShouldThrowException()
        //{
        //    var pastExpirationDate = DateTime.Now.AddHours(-10);
            
        //    Assert.Throws<InvalidFoodItemException>(() => new FoodItem(
        //       "foodItem",
        //       10,
        //       Unit.Pieces,
        //       pastExpirationDate,
        //       Category.Other
        //       ));
        //}

        //[Test]
        //public void CreatedFoodItemWithFutureDateShouldNotThrowException()
        //{
        //    var futureExpirationDate = DateTime.Now.AddHours(10);

        //    Assert.DoesNotThrow(() => new FoodItem(
        //       "foodItem",
        //       10,
        //       Unit.Pieces,
        //       futureExpirationDate,
        //       Category.Other
        //       ));
        //}

        //[Test]
        //public void CreatedFoodItemWithDateTimeNowShouldThrowException()
        //{
        //    var actualExpirationDate = DateTime.Now;
            
        //    Assert.Throws<InvalidFoodItemException>(() => new FoodItem(
        //       "foodItem",
        //       10,
        //       Unit.Pieces,
        //       actualExpirationDate,
        //       Category.Other
        //       ));
        //}
    }
}
