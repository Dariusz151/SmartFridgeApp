using System;
using System.Linq;
using NUnit.Framework;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.FoodProducts.Events;
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
        public void FoodProduct_CreateNew_ShouldHaveCorrectFormattedName(string productName)
        {
            FoodProduct foodProduct = new FoodProduct(productName, category);
            Assert.AreEqual(foodProduct.Name, "Mleko");
        }

        [Test]
        public void FoodProduct_CreateNew_ShouldHaveDomainEvent()
        {
            foodProduct = new FoodProduct("Bułka", category);

            Assert.AreEqual(1, foodProduct.DomainEvents.Count);
            Assert.AreEqual(typeof(FoodProductAddedEvent), foodProduct.DomainEvents.ElementAt(0).GetType());
        }

        [Test]
        public void FoodProduct_CreateNewWithEmptyString_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => foodProduct = new FoodProduct(String.Empty, category));
        }

        [Test]
        public void FoodProduct_UpdateWithEmptyString_ShouldThrowException()
        {
            foodProduct = new FoodProduct("Bułka", category);

            Assert.Throws(typeof(InvalidInputException), () => foodProduct.UpdateProductName(""));
            Assert.AreEqual(foodProduct.Name, "Bułka");
        }

        [Test]
        public void FoodProduct_UpdateCategoryWitInvalidCategory_ShouldThrowException()
        {
            foodProduct = new FoodProduct("Bułka", category);
            var newCategory = new Category(String.Empty);

            Assert.Throws(typeof(InvalidInputException), () => foodProduct.UpdateProductCategory(newCategory));
        }

        [Test]
        public void FoodProduct_UpdateName_ShouldChangeItsName()
        {
            foodProduct = new FoodProduct("Bułka", category);

            foodProduct.UpdateProductName("Kurczak");

            Assert.AreEqual(foodProduct.Name, "Kurczak");
        }

        [Test]
        public void FoodProduct_UpdateCategory_ShouldChangeItsCategory()
        {
            foodProduct = new FoodProduct("Bułka", category);
            var newCategory = new Category("Warzywa");
            foodProduct.UpdateProductCategory(newCategory);

            Assert.AreEqual(foodProduct.Category, newCategory);
        }

        [Test]
        public void FoodProduct_UpdateCategory_ShouldChangeItsCategoryAndName()
        {
            foodProduct = new FoodProduct("Bułka", category);

            var newCategory = new Category("Warzywa");
            foodProduct.UpdateFoodProduct("Kurczak", newCategory);

            Assert.AreEqual(foodProduct.Category, newCategory);
            Assert.AreEqual(foodProduct.Name, "Kurczak");
        }
    }
}
