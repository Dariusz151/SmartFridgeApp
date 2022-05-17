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
using SmartFridgeApp.Core.Application.Features;
using Newtonsoft.Json;
using System.Text;

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
            { "Mięso", "Tłuszcze", "Orzechy i nasiona", "Ryby i owoce morza", "Pieczywo", "Warzywa", "Owoce", "Napoje",
            "Słodycze", "Nabiał i jaja", "Makarony, kasze i ryże", 
            "Inne" };

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

        [Fact]
        public async Task GivenFoodProductsController_WhenCreateNewCategory_CategoriesTableShouldHave1Element()
        {
            CreateCategoryRequest request = new CreateCategoryRequest();
            request.Name = "Category1";

            string jsonObject = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            await _client.PostAsync("http://localhost/api/foodproducts/categories", content);

            await Task.Delay(TimeSpan.FromSeconds(10));

            var model = await _client.GetFromJsonAsync<List<ExpectedFoodProductCategoryModel>>("");
            var modelArray = model.Select(x => x.Name).ToArray();

            Assert.Contains("Category1", modelArray);
        }
    }
}
