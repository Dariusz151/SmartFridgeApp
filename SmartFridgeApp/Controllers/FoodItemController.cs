using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.Domain.Requests;
using SmartFridgeApp.Domain.Requests.Commands;
using SmartFridgeApp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Ping()
        {
            return new string[] { "FoodItemController"};
        }
        
        [HttpPost]
        public async Task<IActionResult> ConsumeFoodItem([FromBody] ConsumeFoodItemRequest request)
        {
            var userId = new Guid("48577875-4D33-4543-8440-8321EB4BAAA3");

            var command = new ConsumeFoodItemCommand(request.FoodItemId, userId);

            await _foodItemService.ConsumeFoodItemAsync(command);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodItem([FromBody] AddFoodItemRequest request)
        {
            var userId = new Guid("48577875-4D33-4543-8440-8321EB4BAAA3");

            var command = new AddFoodItemCommand(request.Name,
                                        userId,
                                        request.Amount,
                                        request.Unit,
                                        request.Category,
                                        request.ExpirationDate);

            await _foodItemService.AddFoodItemAsync(command);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFoodItem([FromBody] DeleteFoodItemRequest request)
        {
            var userId = new Guid("48577875-4D33-4543-8440-8321EB4BAAA3");

            var command = new DeleteFoodItemCommand(request.Id, userId);

            await _foodItemService.DeleteFoodItemAsync(command);

            return Ok();
        }
    }
}
