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
  
    public class FridgesTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public FridgesTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new System.Uri("http://localhost/api/fridges");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllFridges_ReturnsSomeContent()
        {
            var model = await _client.GetFromJsonAsync<List<ExpectedFridgeModel>>("");
            var modelArray = model.Select(x => x.Name).ToArray();

            Assert.True(modelArray.Length > 0);
        }
    }
}
