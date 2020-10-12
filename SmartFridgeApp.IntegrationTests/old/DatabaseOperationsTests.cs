//using System;
//using System.Collections.Generic;
//using System.Linq;
//using FluentAssertions;
//using NUnit.Framework;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using SmartFridgeApp.API.FridgeItems;
//using SmartFridgeApp.API.Fridges;
//using SmartFridgeApp.API.Fridges.AddFridge;
//using SmartFridgeApp.API.Fridges.DeleteFridge;

//namespace SmartFridgeApp.IntegrationTests
//{
//    [TestFixture]
//    public class DatabaseOperationsTests : BaseIntegrationTest
//    {
//        private FridgeDto fridgeDto { get; set; }
//        private FridgeDto fridgeDto2 { get; set; }

//        [SetUp]
//        public async Task BaseSetUp()
//        {
//            var request = new AddFridgeRequest { Name = "Fridge1", Address = "FridgeAddress1", Desc = "FridgeDesc1" };
//            var response = await client.PostAsJsonAsync("/api/fridges", request);
//            var result = response.Content.ReadAsStringAsync().Result;

//            fridgeDto = JsonConvert.DeserializeObject<FridgeDto>(result);

//            request = new AddFridgeRequest { Name = "Fridge2", Address = "FridgeAddress2", Desc = "FridgeDesc2" };
//            response = await client.PostAsJsonAsync("/api/fridges", request);
//            result = response.Content.ReadAsStringAsync().Result;

//            fridgeDto2 = JsonConvert.DeserializeObject<FridgeDto>(result);
//        }

//        [TearDown]
//        public async Task BaseTearDown()
//        {
//            var deleteRequest = new DeleteFridgeRequest {FridgeId = fridgeDto.Id};

//            var request = new HttpRequestMessage
//            {
//                Method = HttpMethod.Delete,
//                RequestUri = new Uri("http://localhost:5001/api/fridges"),
//                Content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8,
//                    "application/json")
//            };
            
//            await client.SendAsync(request);

//            deleteRequest = new DeleteFridgeRequest {FridgeId = fridgeDto2.Id};
//            request = new HttpRequestMessage
//            {
//                Method = HttpMethod.Delete,
//                RequestUri = new Uri("http://localhost:5001/api/fridges"),
//                Content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8,
//                    "application/json")
//            };

//            await client.SendAsync(request);
//        }


//        [Test]
//        public async Task AddFridgeShouldCreateRecordInDatabase()
//        {
//            var response = await client.GetAsync("/api/fridges");
//            var result = response.Content.ReadAsStringAsync().Result;
//            var fridges = JsonConvert.DeserializeObject<IEnumerable<FridgeDto>>(result);

//            foreach (var el in fridges)
//            {
//                Console.WriteLine(el.Name);
//            }

//            var fridgesCount = fridges.Count();

//            Assert.AreEqual(2, fridgesCount);
//            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        }

//        //[Test]
//        //public async Task GetAllFoodProductsShouldReturnsOk()
//        //{
//        //    var response = await client.GetAsync("/api/foodProducts");

//        //    response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        //}

//        //[Test]
//        //public async Task GetAllFridgeItemsShouldReturnsOk()
//        //{
//        //    var userId = "74F61232-41EE-44C6-A274-8600228B48CB";
//        //    var fridgeId = "B518063A-5844-4568-94CF-F1127AB65147";

//        //    var response = await client.GetAsync($"/api/fridgeItems/{fridgeId}/{userId}");

//        //    response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        //}

//        //[Test]
//        //public async Task GetAllFridgeUsersShouldReturnsOk()
//        //{
//        //    var fridgeId = "B518063A-5844-4568-94CF-F1127AB65147";
//        //    var response = await client.GetAsync($"/api/fridgeUsers/{fridgeId}");

//        //    response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        //}

//        //[Test]
//        //public async Task GetAllRecipesShouldReturnsOk()
//        //{
//        //    var response = await client.GetAsync($"/api/recipes");

//        //    response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        //}
        
//        //[Test]
//        //public async Task GetAllFridgesShouldReturnListOfFridges()
//        //{
//        //    var response = await client.GetAsync("/api/fridges");

//        //    var content = await response.Content.ReadAsStringAsync();
//        //    var list = JsonConvert.DeserializeObject<List<FridgeItemDto>>(content);
            
//        //    list.Should().HaveCountGreaterThan(0);
//        //}

//        //[Test]
//        //public async Task AddFridgeItem()
//        //{
//        //    var repository = serviceProvider.GetService<IFridgeRepository>();
//        //    var allFridges = await repository.GetAllAsync();

//        //    foreach (var fridge in allFridges)
//        //        await repository.DeleteAsync(fridge.Id);
//        //    var response = await client.GetAsync("/api/fridges");

//        //    response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        //}
//    }
//}
