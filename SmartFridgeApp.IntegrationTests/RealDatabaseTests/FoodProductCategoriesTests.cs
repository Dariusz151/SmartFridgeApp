
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

    public class FoodProductCategoriesTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public FoodProductCategoriesTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new System.Uri("http://localhost/api/foodproducts/categories");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllFoodProductCategories_ReturnsSomeContent()
        {
            var expected = new List<string> 
            { "Mięso", "Warzywa", "Owoce", 
            "Słodycze", "Nabiał", "Makarony", 
            "Ryże", "Inne" };

            // asserts Content, media types etc. in one method
            var model = await _client.GetFromJsonAsync<List<ExpectedFoodProductCategoryModel>>("");
            var modelArray = model.Select(x => x.Name).ToArray();

            Assert.True(modelArray.Length > 0);
            Assert.Equal(expected.OrderBy(s => s), modelArray.OrderBy(x => x));
        }

        [Fact]
        public async Task GetAllFoodProductCategories_SetsExpectedCacheControlHeader()
        {
            var response = await _client.GetAsync("");
            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(5), header.MaxAge);
            Assert.True(header.Public);
        }
    }
}
