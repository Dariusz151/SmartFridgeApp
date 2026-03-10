using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Core.Exceptions;

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
            category = new Category("Nabia�");
        }

        [TestCase("mleko")]
        [TestCase("MLEKO")]
        [TestCase("mLEKO")]
        [TestCase("mLekO")]
        [TestCase("Mleko")]
        public void FoodProduct_CreateNew_ShouldHaveCorrectFormattedName(string productName)
        {
            FoodProduct foodProduct = new FoodProduct(productName, category);
            ClassicAssert.AreEqual(foodProduct.Name, "Mleko");
        }

        [Test]
        public void FoodProduct_CreateNewWithEmptyString_ShouldThrowException()
        {
            Assert.Throws(typeof(InvalidInputException), () => foodProduct = new FoodProduct(String.Empty, category));
        }

        [Test]
        public void FoodProduct_UpdateWithEmptyString_ShouldThrowException()
        {
            foodProduct = new FoodProduct("Bu�ka", category);

            Assert.Throws(typeof(InvalidInputException), () => foodProduct.UpdateProductName(""));
            ClassicAssert.AreEqual(foodProduct.Name, "Bu�ka");
        }

        [Test]
        public void FoodProduct_UpdateCategoryWitInvalidCategory_ShouldThrowException()
        {
            foodProduct = new FoodProduct("Bu�ka", category);
            var newCategory = new Category(String.Empty);

            Assert.Throws(typeof(InvalidInputException), () => foodProduct.UpdateProductCategory(newCategory));
        }

        [Test]
        public void FoodProduct_UpdateName_ShouldChangeItsName()
        {
            foodProduct = new FoodProduct("Bu�ka", category);

            foodProduct.UpdateProductName("Kurczak");

            ClassicAssert.AreEqual(foodProduct.Name, "Kurczak");
        }

        [Test]
        public void FoodProduct_UpdateCategory_ShouldChangeItsCategory()
        {
            foodProduct = new FoodProduct("Bu�ka", category);
            var newCategory = new Category("Warzywa");
            foodProduct.UpdateProductCategory(newCategory);

            ClassicAssert.AreEqual(foodProduct.Category, newCategory);
        }

        [Test]
        public void FoodProduct_UpdateCategory_ShouldChangeItsCategoryAndName()
        {
            foodProduct = new FoodProduct("Bu�ka", category);

            var newCategory = new Category("Warzywa");
            foodProduct.UpdateFoodProduct("Kurczak", newCategory);

            ClassicAssert.AreEqual(foodProduct.Category, newCategory);
            ClassicAssert.AreEqual(foodProduct.Name, "Kurczak");
        }
    }
}
