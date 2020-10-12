//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using SmartFridgeApp.API.FoodProducts;

//namespace SmartFridgeApp.IntegrationTests.ControllerTests
//{
//    [TestFixture]
//    public class FridgeControllerTest : BaseIntegrationTest
//    {
//        [Test]
//        public async Task GetAllFridges_ReturnsListOfFridges()
//        {
//            var response = await client.GetAsync("/api/fridges");
//            var result = response.Content.ReadAsStringAsync().Result;

//            //var foodProducts = JsonConvert.DeserializeObject<IEnumerable<FoodProductDto>>(result);

//            //foreach (var el in foodProducts)
//            //{
//            //    Console.WriteLine($"[{el.FoodProductId}] {el.FoodProductName} -> {el.FoodProductCategory}");
//            //}
//            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//        }
//    }
//}
