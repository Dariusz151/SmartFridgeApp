using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Fridges;

namespace SmartFridgeApp.IntegrationTests
{
    [TestFixture]
    public class ControllerTests : BaseIntegrationTest
    {
        [Test]
        public async Task GetNotFound()
        {
            var repository = serviceProvider.GetService<IFridgeRepository>();
            var allFridges = await repository.GetAllAsync();

            foreach (var fridge in allFridges)
                await repository.DeleteAsync(fridge.Id);
            var response = await client.GetAsync("/api/fridges");
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
