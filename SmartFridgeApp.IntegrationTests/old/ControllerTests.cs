//using System.Collections.Generic;
//using FluentAssertions;
//using NUnit.Framework;
//using System.Net;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using SmartFridgeApp.API.FridgeItems;

//namespace SmartFridgeApp.IntegrationTests
//{
//    //[TestFixture]
//    //public class ControllerTests : BaseIntegrationTest
//    //{
//    //    [Test]
//    //    public async Task GetAllFridgesShouldReturnsOk()
//    //    {
//    //        var response = await client.GetAsync("/api/fridges");

//    //        response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//    //    }

//    //    [Test]
//    //    public async Task GetAllFoodProductsShouldReturnsOk()
//    //    {
//    //        var response = await client.GetAsync("/api/foodProducts");

//    //        response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//    //    }

//    //    [Test]
//    //    public async Task GetAllFridgeItemsShouldReturnsOk()
//    //    {
//    //        var userId = "74F61232-41EE-44C6-A274-8600228B48CB";
//    //        var fridgeId = "B518063A-5844-4568-94CF-F1127AB65147";

//    //        var response = await client.GetAsync($"/api/fridgeItems/{fridgeId}/{userId}");

//    //        response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//    //    }

//    //    [Test]
//    //    public async Task GetAllFridgeUsersShouldReturnsOk()
//    //    {
//    //        var fridgeId = "B518063A-5844-4568-94CF-F1127AB65147";
//    //        var response = await client.GetAsync($"/api/fridgeUsers/{fridgeId}");

//    //        response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//    //    }

//    //    [Test]
//    //    public async Task GetAllRecipesShouldReturnsOk()
//    //    {
//    //        var response = await client.GetAsync($"/api/recipes");

//    //        response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
//    //    }
        
//    //    [Test]
//    //    public async Task GetAllFridgesShouldReturnListOfFridges()
//    //    {
//    //        var response = await client.GetAsync("/api/fridges");

//    //        var content = await response.Content.ReadAsStringAsync();
//    //        var list = JsonConvert.DeserializeObject<List<FridgeItemDto>>(content);
            
//    //        list.Should().HaveCountGreaterThan(0);
//    //    }

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
//    //}
//}
