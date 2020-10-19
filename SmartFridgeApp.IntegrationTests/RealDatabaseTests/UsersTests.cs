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

    public class UsersTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UsersTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetAllUsers_ReturnsSomeContent()
        {
            var fridgesModel = await _client.GetFromJsonAsync<List<ExpectedFridgeModel>>("http://localhost/api/fridges");
            string userId = fridgesModel.Select(x => x.Id).FirstOrDefault().ToString();

            var usersModel = await _client.GetFromJsonAsync<List<ExpectedUserModel>>($"http://localhost/api/fridgeUsers/{userId}");
            var usersModelArray = usersModel.Select(x => x.Name).ToArray();

            Assert.True(usersModelArray.Length > 0);
        }
    }
}
