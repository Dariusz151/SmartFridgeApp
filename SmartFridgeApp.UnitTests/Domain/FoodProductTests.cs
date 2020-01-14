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

        [TestCase("mleko")]
        [TestCase("MLEKO")]
        [TestCase("mLEKO")]
        [TestCase("mLekO")]
        [TestCase("Mleko")]
        public void CreateNewFoodProductShouldHaveCorrectFormattedName(string productName)
        {
            FoodProduct foodProduct = new FoodProduct(productName);
            Assert.AreEqual(foodProduct.Name, "Mleko");
        }

        [Test]
        public void CreateNewFoodProductWithEmptyStringShouldThrowException()
        {
            FoodProduct foodProduct;
            Assert.Throws(typeof(DomainException), () => foodProduct = new FoodProduct(String.Empty));
        }
    }
}
