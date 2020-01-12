using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Fridges;
using Newtonsoft.Json;
using SmartFridgeApp.API.FridgeItems;

namespace SmartFridgeApp.IntegrationTests
{
    [TestFixture]
    public class ControllerTests : BaseIntegrationTest
    {
        [Test]
        public async Task GetAllFridgesShouldReturnsOk()
        {
            var response = await client.GetAsync("/api/fridges");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Test]
        public async Task GetAllFridgesShouldReturnListOfFridges()
        {
            var response = await client.GetAsync("/api/fridges");

            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<FridgeItemDto>>(content);
            
            list.Should().HaveCountGreaterThan(0);
        }

        //[Test]
        //public async Task AddFridgeItem()
        //{
        //    var repository = serviceProvider.GetService<IFridgeRepository>();
        //    var allFridges = await repository.GetAllAsync();

        //    foreach (var fridge in allFridges)
        //        await repository.DeleteAsync(fridge.Id);
        //    var response = await client.GetAsync("/api/fridges");

        //    response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        //}
    }
}
