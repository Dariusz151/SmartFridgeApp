using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFridgeApp.Core.Domain.DomainServices;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;

namespace SmartFridgeApp.UnitTests.Domain;

[TestFixture]
public class RecipeMatcherTests
{
    private static FoodProductDetails Product(short id, bool isOptional = false)
    {
        var fp = new FoodProductDetails(id, $"Product{id}", new AmountValue(1f, Unit.Pieces));
        if (isOptional) fp.SetOptional();
        return fp;
    }

    private static Recipe CreateRecipe(string name, params FoodProductDetails[] products)
    {
        return new Recipe(name, new List<FoodProductDetails>(products));
    }

    [Test]
    public void FindAvailable_NoSelection_ReturnsAllRecipesPossibleWithInventory()
    {
        var recipe1 = CreateRecipe("R1", Product(1), Product(2));
        var recipe2 = CreateRecipe("R2", Product(3));
        var recipes = new List<Recipe> { recipe1, recipe2 };
        var available = new HashSet<short> { 1, 2, 3 };

        var result = RecipeMatcher.FindAvailable(recipes, available, []);

        ClassicAssert.AreEqual(2, result.Count);
    }

    [Test]
    public void FindAvailable_NoSelection_ExcludesRecipesWithMissingRequiredProducts()
    {
        var recipe1 = CreateRecipe("R1", Product(1), Product(2));
        var recipe2 = CreateRecipe("R2", Product(3));
        var recipes = new List<Recipe> { recipe1, recipe2 };
        var available = new HashSet<short> { 1, 3 }; // missing product 2

        var result = RecipeMatcher.FindAvailable(recipes, available, []);

        ClassicAssert.AreEqual(1, result.Count);
        ClassicAssert.AreEqual("R2", result[0].Name);
    }

    [Test]
    public void FindAvailable_WithSelection_ReturnsOnlyRecipesContainingSelectedProducts()
    {
        var recipe1 = CreateRecipe("R1", Product(1), Product(2));
        var recipe2 = CreateRecipe("R2", Product(3));
        var recipes = new List<Recipe> { recipe1, recipe2 };
        var available = new HashSet<short> { 1, 2, 3 };

        var result = RecipeMatcher.FindAvailable(recipes, available, [1]);

        ClassicAssert.AreEqual(1, result.Count);
        ClassicAssert.AreEqual("R1", result[0].Name);
    }

    [Test]
    public void FindAvailable_WithSelection_StillRequiresAllProductsInInventory()
    {
        var recipe1 = CreateRecipe("R1", Product(1), Product(2));
        var recipes = new List<Recipe> { recipe1 };
        var available = new HashSet<short> { 1 }; // missing product 2

        var result = RecipeMatcher.FindAvailable(recipes, available, [1]);

        ClassicAssert.AreEqual(0, result.Count);
    }

    [Test]
    public void FindAvailable_OptionalProductsMissing_RecipeStillReturned()
    {
        var recipe = CreateRecipe("R1", Product(1), Product(2, isOptional: true));
        var recipes = new List<Recipe> { recipe };
        var available = new HashSet<short> { 1 }; // optional product 2 missing

        var result = RecipeMatcher.FindAvailable(recipes, available, []);

        ClassicAssert.AreEqual(1, result.Count);
    }

    [Test]
    public void FindAvailable_NoRecipesMatch_ReturnsEmptyList()
    {
        var recipe = CreateRecipe("R1", Product(1), Product(2));
        var recipes = new List<Recipe> { recipe };
        var available = new HashSet<short> { 5, 6 };

        var result = RecipeMatcher.FindAvailable(recipes, available, []);

        ClassicAssert.AreEqual(0, result.Count);
    }

    [Test]
    public void FindAvailable_EmptyRecipeList_ReturnsEmptyList()
    {
        var result = RecipeMatcher.FindAvailable([], new HashSet<short> { 1 }, []);

        ClassicAssert.AreEqual(0, result.Count);
    }

    [Test]
    public void FindMissing_AllProductsAvailable_ReturnsEmptyList()
    {
        var recipe = CreateRecipe("R1", Product(1), Product(2));
        var available = new HashSet<short> { 1, 2 };

        var result = RecipeMatcher.FindMissing(recipe, available);

        ClassicAssert.AreEqual(0, result.Count);
    }

    [Test]
    public void FindMissing_SomeProductsMissing_ReturnsMissingProducts()
    {
        var recipe = CreateRecipe("R1", Product(1), Product(2), Product(3));
        var available = new HashSet<short> { 1 };

        var result = RecipeMatcher.FindMissing(recipe, available);

        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.IsTrue(result.Exists(x => x.FoodProductId == 2));
        ClassicAssert.IsTrue(result.Exists(x => x.FoodProductId == 3));
    }

    [Test]
    public void FindMissing_OptionalProductsMissing_NotCountedAsMissing()
    {
        var recipe = CreateRecipe("R1", Product(1), Product(2, isOptional: true));
        var available = new HashSet<short> { 1 };

        var result = RecipeMatcher.FindMissing(recipe, available);

        ClassicAssert.AreEqual(0, result.Count);
    }

    [Test]
    public void FindMissing_AllProductsMissing_ReturnsAllRequired()
    {
        var recipe = CreateRecipe("R1", Product(1), Product(2), Product(3, isOptional: true));
        var available = new HashSet<short>();

        var result = RecipeMatcher.FindMissing(recipe, available);

        ClassicAssert.AreEqual(2, result.Count);
        ClassicAssert.IsTrue(result.Exists(x => x.FoodProductId == 1));
        ClassicAssert.IsTrue(result.Exists(x => x.FoodProductId == 2));
    }
}
