using Microsoft.AspNetCore.Mvc.Testing;
using SmartFridgeApp.API;
using SmartFridgeApp.IntegrationTests.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Net.Http.Json;
using System;

namespace SmartFridgeApp.IntegrationTests.RealDatabaseTests
{
   
    public class RecipeCategoryTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public RecipeCategoryTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new System.Uri("http://localhost/api/recipes/categories");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllRecipeCategories_ReturnsSomeContent()
        {
            var expected = new List<string>
            { "Obiad", "Kolacja", "Śniadanie",
            "Inne", "Przystawka", "Deser" };

            var model = await _client.GetFromJsonAsync<List<ExpectedRecipeCategoryModel>>("");
            var modelArray = model.Select(x => x.Name).ToArray();

            Assert.True(modelArray.Length > 0);
            Assert.Equal(expected.OrderBy(s => s), modelArray.OrderBy(x => x));
        }
    }
}
