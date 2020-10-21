using SmartFridgeApp.Domain.Models.FoodProducts.Events;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.SeedWork.Exceptions;

namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public class FoodProduct : Entity, IAggregateRoot
    {
        public short FoodProductId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }

        private FoodProduct()
        {
            // EF Core
        }

        public FoodProduct(string name, Category category)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Product name can't be empty.");
            Name = UppercaseFirst(name.ToLower());

            Category = category;

            //this.AddDomainEvent(new FoodProductAddedEvent(this));
        }

        public void UpdateFoodProduct(string newName, Category category)
        {
            this.UpdateProductName(newName);
            this.UpdateProductCategory(category);
        }

        public void UpdateProductName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new DomainException("Product name can't be empty.");
            Name = UppercaseFirst(newName);

            //this.AddDomainEvent(new FoodProductChangedEvent(this));
        }

        public void UpdateProductCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
                throw new DomainException("Product must have category with name!.");
            Category = category;

            //this.AddDomainEvent(new FoodProductChangedEvent(this));
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
