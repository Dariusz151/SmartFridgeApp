using System;
using NUnit.Framework;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class FoodProductTests
    {
        Category category;
        FoodProduct foodProduct;
        [SetUp]
        public void BaseSetUp()
        {
            category = new Category("Nabiał");
        }

        [TestCase("mleko")]
        [TestCase("MLEKO")]
        [TestCase("mLEKO")]
        [TestCase("mLekO")]
        [TestCase("Mleko")]
        public void CreateNewFoodProductShouldHaveCorrectFormattedName(string productName)
        {
            FoodProduct foodProduct = new FoodProduct(productName, category);
            Assert.AreEqual(foodProduct.Name, "Mleko");
        }

        [Test]
        public void CreateNewFoodProductWithEmptyStringShouldThrowException()
        {
            Assert.Throws(typeof(DomainException), () => foodProduct = new FoodProduct(String.Empty, category));
        }

        [Test]
        public void UpdateFoodProductWithEmptyStringShouldThrowException()
        {
            foodProduct = new FoodProduct("Bułka", category);

            Assert.Throws(typeof(DomainException), () => foodProduct.UpdateProductName(""));
            Assert.AreEqual(foodProduct.Name, "Bułka");
        }

        [Test]
        public void UpdateFoodProductCategoryWitInvalidCategory_ShouldThrowException()
        {
            foodProduct = new FoodProduct("Bułka", category);
            var newCategory = new Category(String.Empty);

            Assert.Throws(typeof(DomainException), () => foodProduct.UpdateProductCategory(newCategory));
        }

        [Test]
        public void UpdateFoodProductName_ShouldChangeItsName()
        {
            foodProduct = new FoodProduct("Bułka", category);

            foodProduct.UpdateProductName("Kurczak");

            Assert.AreEqual(foodProduct.Name, "Kurczak");
        }

        [Test]
        public void UpdateFoodProductCategory_ShouldChangeItsCategory()
        {
            foodProduct = new FoodProduct("Bułka", category);
            var newCategory = new Category("Warzywa");
            foodProduct.UpdateProductCategory(newCategory);

            Assert.AreEqual(foodProduct.Category, newCategory);
        }

        [Test]
        public void UpdateFoodProduct_ShouldChangeItsCategoryAndName()
        {
            foodProduct = new FoodProduct("Bułka", category);

            var newCategory = new Category("Warzywa");
            foodProduct.UpdateFoodProduct("Kurczak", newCategory);

            Assert.AreEqual(foodProduct.Category, newCategory);
            Assert.AreEqual(foodProduct.Name, "Kurczak");
        }
    }
}
