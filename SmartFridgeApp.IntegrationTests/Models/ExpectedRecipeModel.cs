using System.Collections.Generic;

namespace SmartFridgeApp.IntegrationTests.Models
{
    public class ExpectedRecipeModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ExpectedFoodProductModel> FoodProducts { get; set; }
    }
}
