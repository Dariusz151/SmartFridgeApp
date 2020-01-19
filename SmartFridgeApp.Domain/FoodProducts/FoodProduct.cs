using System.Collections.Generic;
using SmartFridgeApp.Domain.Recipes;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.Domain.FoodProducts
{
    public class FoodProduct : Entity, IAggregateRoot
    {
        public int FoodProductId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }

        private FoodProduct()
        {
            // EF Core
        }

        public FoodProduct(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Product name can't be empty.");
            Name = UppercaseFirst(name.ToLower());
        }
        
        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
