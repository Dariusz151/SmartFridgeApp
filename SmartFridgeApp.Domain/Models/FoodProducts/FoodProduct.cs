using System.Collections.Generic;
using SmartFridgeApp.Domain.Models.FoodProducts.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class FoodProduct : Entity, IAggregateRoot
    {
        public int FoodProductId { get; private set; }
        public string Name { get; private set; }

        //public ICollection<RecipeFoodProduct> RecipeFoodProducts { get; set; }

        private FoodProduct()
        {
            // EF Core
        }

        public FoodProduct(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Product name can't be empty.");
            Name = UppercaseFirst(name.ToLower());

            this.AddDomainEvent(new FoodProductAddedEvent(this));
        }

        public void UpdateProductName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new DomainException("Product name can't be empty.");
            Name = UppercaseFirst(newName);

            this.AddDomainEvent(new FoodProductChangedEvent(this));
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
