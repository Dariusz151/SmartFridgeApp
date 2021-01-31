using Microsoft.AspNetCore.Mvc.Testing;
using SmartFridgeApp.API;
using SmartFridgeApp.IntegrationTests.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using System;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.IntegrationTests.RealDatabaseTests
{

    public class FridgeItemsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public FridgeItemsTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task GivenScenario_AddFridgeitemsToUser_UserHasFridgeItemsInDatabase()
        {
            AddFridgeRequest fridgeRequest = new AddFridgeRequest();
            fridgeRequest.Name = "TestFridge";
            fridgeRequest.Address = "TestAddress";
            fridgeRequest.Desc = "TestDesc";

            string jsonObject = JsonConvert.SerializeObject(fridgeRequest);
            StringContent content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            await _client.PostAsync("http://localhost/api/fridges", content);

            var fridgesModel = await _client.GetFromJsonAsync<List<ExpectedFridgeModel>>("http://localhost/api/fridges");
            var fridge = fridgesModel.Where(x => x.Name == "TestFridge").FirstOrDefault();

            AddFridgeUserRequest userRequest = new AddFridgeUserRequest();
            UserDto userDto = new UserDto();
            userDto.Name = "TestUser";
            userDto.Email = "TestEmail";
            userRequest.User = userDto;

            jsonObject = JsonConvert.SerializeObject(userRequest);
            content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            await _client.PostAsync($"http://localhost/api/fridgeusers/{fridge.Id}", content);

            var usersModel = await _client.GetFromJsonAsync<List<ExpectedUserModel>>($"http://localhost/api/fridgeUsers/{fridge.Id}");
            var usersModelArray = usersModel.Where(x => x.Name == "TestUser").FirstOrDefault();

            AddFridgeItemDto fridgeItem = new AddFridgeItemDto();
            AddFridgeItemRequest fridgeItemRequest = new AddFridgeItemRequest();

            fridgeItemRequest.UserId = usersModelArray.Id;
            fridgeItem.FoodProductId = 1;
            fridgeItem.Note = "TestNote";
            fridgeItem.Unit = Core.Domain.Shared.Unit.Grams;
            fridgeItem.Value = 10.0f;

            fridgeItemRequest.FridgeItem = fridgeItem;

            jsonObject = JsonConvert.SerializeObject(fridgeItemRequest);
            content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            await _client.PostAsync($"http://localhost/api/fridgeitems/{fridge.Id}/add", content);

            fridgeItem.FoodProductId = 3;
            fridgeItem.Note = "TestNote2";
            fridgeItem.Unit = Core.Domain.Shared.Unit.Grams;
            fridgeItem.Value = 20.0f;

            fridgeItemRequest.FridgeItem = fridgeItem;

            jsonObject = JsonConvert.SerializeObject(fridgeItemRequest);
            content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            await _client.PostAsync($"http://localhost/api/fridgeitems/{fridge.Id}/add", content);

            fridgeItem.Note = "TestNote3";
            fridgeItem.FoodProductId = 6;
            fridgeItem.Unit = Core.Domain.Shared.Unit.Grams;
            fridgeItem.Value = 30.0f;

            fridgeItemRequest.FridgeItem = fridgeItem;

            jsonObject = JsonConvert.SerializeObject(fridgeItemRequest);
            content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            await _client.PostAsync($"http://localhost/api/fridgeitems/{fridge.Id}/add", content);

            var fridgeItemsModel = await _client.GetFromJsonAsync<List<ExpectedFridgeItemModel>>($"http://localhost/api/fridgeitems/{fridge.Id}/{usersModelArray.Id}");
          
            Assert.Equal(3, fridgeItemsModel.Count);

            //TODO : Delete all added objects in database


            var fridgeItems = fridgeItemsModel.Where(x => x.Note.Contains("Test")).ToList();
            foreach (var el in fridgeItems)
            {
                RemoveFridgeItemRequest removeRequest = new RemoveFridgeItemRequest();
                removeRequest.FridgeItemId = el.Id;
                removeRequest.UserId = usersModelArray.Id;

                jsonObject = JsonConvert.SerializeObject(removeRequest);

                HttpRequestMessage request = new HttpRequestMessage
                {
                    Content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"http://localhost/api/fridgeitems/{fridge.Id}/remove")
                };
                await _client.SendAsync(request);
            }
        }
    }
}
