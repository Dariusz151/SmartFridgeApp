using System;
using NUnit.Framework;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FoodProductTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            
        }

        [Test]
        public void CreateNewFoodProductShouldHaveCorrectName()
        {
            var testString = "test";

            FoodProduct foodProduct = new FoodProduct(testString);
            Assert.AreEqual(foodProduct.Name, testString);
        }
        [Test]
        public void CreateNewFoodProductWithEmptyStringShouldThrowException()
        {
            var testString = String.Empty;

            FoodProduct foodProduct;

            Assert.Throws(typeof(DomainException), () => foodProduct = new FoodProduct(testString));
        }
    }
}
