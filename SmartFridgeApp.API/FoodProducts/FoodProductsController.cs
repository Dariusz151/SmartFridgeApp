using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartFridgeApp.API.Fridges.GetFridges;
using SmartFridgeApp.Domain.FoodProducts;

namespace SmartFridgeApp.API.FoodProducts
{
    [Route("api/foodProducts")]
    [ApiController]
    public class FoodProductsController : Controller
    {
        private readonly IMediator _mediator;

        public FoodProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all available foodProducts.
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<FoodProduct>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllFoodProducts()
        {
            var foodProducts = await _mediator.Send(new GetFridgesQuery());

            return Ok(foodProducts);
        }
    }
}
