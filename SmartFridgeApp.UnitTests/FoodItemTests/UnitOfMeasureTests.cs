using NUnit.Framework;
using SmartFridgeApp.Domain.Exceptions;
using SmartFridgeApp.Domain.Models;
using SmartFridgeApp.Types;
using System;

namespace SmartFridgeApp.UnitTests.FoodItemTests
{
    [TestFixture]
    public class UnitOfMeasureTests
    {
        [Test]
        public void CreateFoodItemWithoutUnitShouldThrowException()
        {
            Assert.Throws<InvalidFoodItemException>(() => new FoodItem(
               "foodItem",
               10,
               Unit.NotAssigned,
               DateTime.UtcNow.AddHours(10),
               Category.Other
               ));
        }

        [Test]
        public void CreateFoodItemWithProperUnitShouldNotThrowException()
        {
            Assert.DoesNotThrow(() => new FoodItem(
               "foodItem",
               10,
               Unit.Pieces,
               DateTime.UtcNow.AddHours(10),
               Category.Other
               ));
        }

    }
}
