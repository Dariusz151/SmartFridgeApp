using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartFridgeApp.API.FoodProducts;
using SmartFridgeApp.API.FoodProducts.AddFoodProduct;
using SmartFridgeApp.API.FoodProducts.DeleteFoodProduct;
using SmartFridgeApp.API.FoodProducts.UpdateFoodProduct;
using SmartFridgeApp.Domain.Models.FoodProducts;

namespace SmartFridgeApp.IntegrationTests.DatabaseTests
{
    [TestFixture]
    public class FoodProductDatabaseTests : BaseIntegrationTest
    {
        [Test]
        public async Task _0_GetAllFoodProductsShouldReturnToConsole()
        {
            var response = await client.GetAsync("/api/foodProducts");
            var result = response.Content.ReadAsStringAsync().Result;
            var foodProducts = JsonConvert.DeserializeObject<IEnumerable<FoodProductDto>>(result);

            foreach (var el in foodProducts)
            {
                Console.WriteLine($"[{el.FoodProductId}] {el.FoodProductName} -> {el.FoodProductCategory}");
            }
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public async Task _1_AddFoodProductShouldHaveGivenInDatabase()
        {
            //string foodProductMock = "FoodProductMock3";

            //var request = new AddFoodProductRequest { Name = foodProductMock, Category = (int)Category.Liquid};
            //await client.PostAsJsonAsync("/api/foodProducts", request);
            
            //var response = await client.GetAsync("/api/foodProducts");
            //var result = response.Content.ReadAsStringAsync().Result;
            //var foodProducts = JsonConvert.DeserializeObject<IEnumerable<FoodProductDto>>(result);

            //int countEqualElement = 0;

            //foreach (var el in foodProducts)
            //{
            //    if (el.FoodProductName.ToLower().Equals(foodProductMock.ToLower()))
            //    {
            //        countEqualElement++;
            //    }
            //}

            //Assert.AreEqual(1, countEqualElement);
            //response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public async Task _2_UpdateFoodProductShouldHaveGivenInDatabase()
        {
            bool isSuccess = false;
            string foodProductMock = "FoodProductMock3";

            var response = await client.GetAsync("/api/foodProducts");
            var result = response.Content.ReadAsStringAsync().Result;
            var foodProducts = JsonConvert.DeserializeObject<IEnumerable<FoodProductDto>>(result);

            foreach (var el in foodProducts)
            {
                if (el.FoodProductName.ToLower().Equals(foodProductMock.ToLower()))
                {
                    var updateRequest = new UpdateFoodProductRequest() { FoodProductId = el.FoodProductId, FoodProductName = "FoodProductMockUpdated"};

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri("http://localhost:5001/api/foodProducts"),
                        Content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8,
                            "application/json")
                    };

                    await client.SendAsync(request);
                    isSuccess = true;
                }
            }
            Assert.AreEqual(true, isSuccess);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public async Task _3_DeleteFoodProductMockUpdatedIfExistShouldReturnNoContent()
        {
            bool isSuccess = false;
            string foodProductMock = "FoodProductMockUpdated";

            var response = await client.GetAsync("/api/foodProducts");
            var result = response.Content.ReadAsStringAsync().Result;
            var foodProducts = JsonConvert.DeserializeObject<IEnumerable<FoodProductDto>>(result);
            
            foreach (var el in foodProducts)
            {
                if (el.FoodProductName.ToLower().Equals(foodProductMock.ToLower()))
                {
                    var deleteRequest = new DeleteFoodProductRequest { FoodProductId = el.FoodProductId };

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("http://localhost:5001/api/foodProducts"),
                        Content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8,
                            "application/json")
                    };

                    await client.SendAsync(request);
                    isSuccess = true;
                }
            }
            Assert.AreEqual(true, isSuccess);
            //response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }
    }
}
