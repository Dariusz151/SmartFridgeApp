using System;
using System.Collections.Generic;
using NUnit.Framework;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork.Exceptions;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class RecipesTests
    {
        RecipeCategory recipeCategory;
        FoodProductDetails foodProductDetails;

        [SetUp]
        public void BaseSetUp()
        {
            recipeCategory = new RecipeCategory("Obiad");
            foodProductDetails = new FoodProductDetails(1, new AmountValue(10.0f, Unit.Mililiter));
        }
        
        [Test]
        public void CreateNewRecipeWithoutProductsShouldThrowException()
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            Assert.Throws(typeof(DomainException),
                () => recipe = new Recipe("recipe", list));
        }

        [Test]
        public void CreateNewRecipeWithoutNameShouldThrowException()
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);

            Assert.Throws(typeof(DomainException),
                () => recipe = new Recipe(String.Empty, list));
        }

        [Test]
        public void CreateNewRecipeWithNameAndProductsShouldBeFine()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);

            recipe = new Recipe("recipe", "desc", list);

            Assert.AreEqual(1, recipe.FoodProducts.Count);
        }

        [Test]
        public void UpdateRecipeWithInvalidDetailsShouldThrowException()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);
            recipe = new Recipe("recipe", "desc", recipeCategory, list, 30, (int)LevelOfDifficulty.Easy);

            Assert.Throws(typeof(DomainException), () => recipe.UpdateRecipe(String.Empty, "descUpdate", recipeCategory, 30, (int)LevelOfDifficulty.Easy));
            Assert.AreEqual(recipe.RecipeCategory.Name, "Obiad");
        }
    }
}
