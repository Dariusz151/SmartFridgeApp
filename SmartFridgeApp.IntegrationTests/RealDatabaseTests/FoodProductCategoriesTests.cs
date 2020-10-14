
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

namespace SmartFridgeApp.IntegrationTests.RealDatabaseTests
{

    public class FoodProductCategoriesTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public FoodProductCategoriesTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetAllFoodProductCategories_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/foodproducts/categories");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAllFoodProductCategories_ReturnsSomeContent()
        {
            var expected = new List<string> 
            { "Mięso", "Warzywa", "Owoce", 
            "Słodycze", "Nabiał", "Makarony", 
            "Ryże", "Inne" };

            var responseStream = await _client.GetStreamAsync("/api/foodproducts/categories");

            var model = await JsonSerializer.DeserializeAsync<List<ExpectedFoodProductCategoriesModel>>(responseStream,
                JsonSerializerHelper.DefaultDeserialisationOptions);

            Assert.Equal(model.Count, expected.Count);
        }
    }
}
