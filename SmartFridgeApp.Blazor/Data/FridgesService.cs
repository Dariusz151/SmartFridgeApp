using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartFridgeApp.Blazor.Data
{
    public class FridgesService
    {
        public async Task<Fridge[]> GetFridgesAsync()
        {
            HttpClient _httpClient;
            _httpClient = new HttpClient();
            var response = await _httpClient.GetAsync("https://localhost:5001/api/fridges");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            
            //return await JsonSerializer.DeserializeAsync
            //    <IEnumerable<string>>(responseStream);

            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new Fridge
            {
                Name = "siema",
                Number = 1
            }).ToArray());
        }
    }
}