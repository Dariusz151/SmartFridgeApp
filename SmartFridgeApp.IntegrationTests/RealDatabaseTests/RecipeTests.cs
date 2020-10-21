using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartFridgeApp.API;
using SmartFridgeApp.IntegrationTests.Models;
using SmartFridgeApp.IntegrationTests.TestHelpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Net.Http.Json;
using System;

namespace SmartFridgeApp.IntegrationTests.RealDatabaseTests
{
    public class RecipeTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public RecipeTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/recipes");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllRecipes_ReturnsListOfRecipes()
        {
            var model = await _client.GetFromJsonAsync<List<ExpectedRecipeModel>>("");
            var firstRecipe = model.FirstOrDefault();

            Assert.True(firstRecipe.Name.Length > 0);
            Assert.True(firstRecipe.FoodProducts.Count > 0);

            Assert.NotNull(model);
            Assert.True(model.Count > 0);
        }
    }
}
