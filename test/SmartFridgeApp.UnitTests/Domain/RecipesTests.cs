using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Core.Exceptions;

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
        public void CreateNewRecipe_WithoutProducts_ShouldThrowException()
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            Assert.Throws(typeof(DomainException),
                () => recipe = new Recipe("recipe", list));
        }

        [Test]
        public void CreateNewRecipe_WithNegativeRequiredTime_ShouldThrowException()
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();
            list.Add(foodProductDetails);

            Assert.Throws(typeof(InvalidInputException),
                () => recipe = new Recipe("recipe", "desc", recipeCategory, list, -1, 1));
        }

        [TestCase(-1)]
        [TestCase(5)]
        [Test]
        public void CreateNewRecipe_WitInvalidLevelOfDifficulty_ShouldThrowException(int levelOfDifficulty)
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();
            list.Add(foodProductDetails);

            Assert.Throws(typeof(DomainException),
                () => recipe = new Recipe("recipe", "desc", recipeCategory, list, 20, levelOfDifficulty));
        }

        [Test]
        public void CreateNewRecipe_WithoutName_ShouldThrowException()
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);

            Assert.Throws(typeof(InvalidInputException),
                () => recipe = new Recipe(String.Empty, list));
        }

        [Test]
        public void ExistingRecipe_UpdateWithInvalidCategoryName_ShouldThrowException()
        {
            Recipe recipe;
            List<FoodProductDetails> list = new List<FoodProductDetails>();
            list.Add(foodProductDetails);

            recipe = new Recipe("recipe", "desc", recipeCategory, list, 20, 2);

            Assert.Throws(typeof(DomainException),
                () => recipe.UpdateRecipeCategory(new RecipeCategory(String.Empty)));
        }

        [Test]
        public void ExistingRecipe_UpdateWithInvalidName_ShouldThrowException()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);
            recipe = new Recipe("recipe", "desc", recipeCategory, list, 30, (int)LevelOfDifficulty.Easy);

            Assert.Throws(typeof(InvalidInputException), () => recipe.UpdateRecipe(String.Empty, "descUpdate", recipeCategory, 30, (int)LevelOfDifficulty.Easy));
        }

        [Test]
        public void ExistingRecipe_UpdateWithInvalidRequiredTime_ShouldThrowException()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);
            recipe = new Recipe("recipe", "desc", recipeCategory, list, 30, (int)LevelOfDifficulty.Easy);

            Assert.Throws(typeof(InvalidInputException), () => recipe.UpdateRecipe("updated", "descUpdate", recipeCategory, -1, (int)LevelOfDifficulty.Hard));
            ClassicAssert.AreEqual("recipe", recipe.Name);
            ClassicAssert.AreEqual("desc", recipe.Description);
            ClassicAssert.AreEqual(LevelOfDifficulty.Easy, recipe.LevelOfDifficulty);
        }

        [Test]
        public void ExistingRecipe_UpdateWithInvalidLevelOfDifficulty_ShouldThrowException()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);
            recipe = new Recipe("recipe", "desc", recipeCategory, list, 30, (int)LevelOfDifficulty.Easy);

            Assert.Throws(typeof(DomainException), () => recipe.UpdateRecipe("updated", "descUpdate", recipeCategory, 20, 5));
            ClassicAssert.AreEqual("recipe", recipe.Name);
            ClassicAssert.AreEqual("desc", recipe.Description);
            ClassicAssert.AreEqual(30, recipe.RequiredTime);
            ClassicAssert.AreEqual(LevelOfDifficulty.Easy, recipe.LevelOfDifficulty);
        }

        [Test]
        public void ExistingRecipe_UpdateWithInvalidDescription_ShouldUpdateAllExceptDesc()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);
            recipe = new Recipe("recipe", "desc", recipeCategory, list, 30, (int)LevelOfDifficulty.Easy);
            recipe.UpdateRecipe("updated", String.Empty, recipeCategory, 20, (int)LevelOfDifficulty.Hard);

            ClassicAssert.AreEqual("updated", recipe.Name);
            ClassicAssert.AreEqual("desc", recipe.Description);
            ClassicAssert.AreEqual(20, recipe.RequiredTime);
            ClassicAssert.AreEqual(LevelOfDifficulty.Hard, recipe.LevelOfDifficulty);
        }

        [Test]
        public void CreateNewRecipe_WithNameAndProducts_ShouldBeFine()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);

            recipe = new Recipe("recipe", list);

            ClassicAssert.AreEqual(1, recipe.FoodProducts.Count);
            ClassicAssert.AreEqual(String.Empty, recipe.Description);
            ClassicAssert.AreEqual("recipe", recipe.Name);
        }

        [Test]
        public void CreateNewRecipe_WithNameDescAndProducts_ShouldBeFine()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);

            recipe = new Recipe("recipe", "desc", list);

            ClassicAssert.AreEqual(1, recipe.FoodProducts.Count);
        }

        [Test]
        public void CreateNewRecipe_WithRequiredTimeandLevelOfDifficulty_ShouldBeFine()
        {
            Recipe recipe = null;
            List<FoodProductDetails> list = new List<FoodProductDetails>();

            list.Add(foodProductDetails);

            recipe = new Recipe("recipe", "desc", recipeCategory, list, 30, (int)LevelOfDifficulty.Hard);

            ClassicAssert.AreEqual(1, recipe.FoodProducts.Count);
            ClassicAssert.AreEqual(30, recipe.RequiredTime);
            ClassicAssert.AreEqual(LevelOfDifficulty.Hard, recipe.LevelOfDifficulty);
        }
    }
}
