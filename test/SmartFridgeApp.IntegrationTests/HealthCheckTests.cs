using Microsoft.AspNetCore.Mvc.Testing;
using SmartFridgeApp.API;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SmartFridgeApp.IntegrationTests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

       [Fact]
       public async Task HealthCheck_Returns_OK()
        {
            var response = await _client.GetAsync("/healthcheck");

            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response.EnsureSuccessStatusCode();
        }
    }
}
