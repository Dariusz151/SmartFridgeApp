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
    public class FoodProductsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public FoodProductsTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/foodproducts");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllFoodProducts_ReturnsListOfFoodProductsLongerThan0()
        {
            var model = await _client.GetFromJsonAsync<List<ExpectedFoodProductModel>>("");

            Assert.NotNull(model);
            Assert.True(model.Count > 0);
        }

        [Fact]
        public async Task GetAllFoodProducts_ReturnedListOfFoodProducts_AllHaveCategories()
        {
            var model = await _client.GetFromJsonAsync<List<ExpectedFoodProductModel>>("");

            Assert.All(model, x => Assert.False(String.IsNullOrEmpty(x.FoodProductCategory)));
        }
    }
}
