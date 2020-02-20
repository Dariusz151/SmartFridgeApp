using System;
using System.Collections.Generic;
using NUnit.Framework;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.UnitTests.Domain
{
    [TestFixture]
    public class RecipesTests
    {
        [SetUp]
        public void BaseSetUp()
        {
            
        }
        
        [Test]
        public void CreateNewRecipeWithoutProductsShouldThrowException()
        {
            Recipe recipe;
            List<FoodProduct> list = new List<FoodProduct>();
            
            Assert.Throws(typeof(DomainException),
                () => recipe = new Recipe("recipe", "desc", "category", list));
        }

        [Test]
        public void CreateNewRecipeWithoutNameShouldThrowException()
        {
            //Recipe recipe;
            //List<FoodProduct> list = new List<FoodProduct>();

            //list.Add(new FoodProduct());

            //Assert.Throws(typeof(DomainException),
            //    () => recipe = new Recipe("", "desc", 2, 25, "category", list));
        }

        [Test]
        public void CreateNewRecipeWithNameAndProductsShouldBeFine()
        {
            //Recipe recipe = null;
            //List<FoodProduct> list = new List<FoodProduct>();

            
            //list.Add(rfp);

            //recipe = new Recipe("recipe", "desc", list);

            //Assert.AreEqual(1, recipe.FoodProducts.Count);
        }

        [Test]
        public void UpdateRecipeWithInvalidDetailsShouldThrowException()
        {
            //Recipe recipe = null;
            //List<FoodProduct> list = new List<FoodProduct>();
            //FoodProduct rfp = new FoodProduct();
            //rfp.FoodProductId = 1;
            //rfp.RecipeId = 1;

            //list.Add(rfp);

            //recipe = new Recipe("recipe", "desc", list);
            
            //Assert.Throws(typeof(DomainException), () => recipe.UpdateRecipe("", "descUpdate", 2, 20, "category"));
            //Assert.Throws(typeof(DomainException), () => recipe.UpdateRecipe("", "", 0, 0, "category"));
            
            //recipe.UpdateRecipe("recipe", "", 0, 0, "");
            //Assert.AreEqual("recipe", recipe.Name);
            //Assert.AreEqual("desc", recipe.Description);

            //recipe.UpdateRecipe("recipeUpdated", "descUpdated", 5, 50, "");

            //Assert.AreEqual("recipeUpdated", recipe.Name);
            //Assert.AreEqual("descUpdated", recipe.Description);
            //Assert.AreEqual(5, recipe.DifficultyLevel);
            //Assert.AreEqual(50, recipe.MinutesRequired);
        }
    }
}
