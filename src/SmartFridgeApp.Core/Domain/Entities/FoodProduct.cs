using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Domain.Entities
{
    public class FoodProduct : Entity, IAggregateRoot
    {
        public short FoodProductId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }

        private FoodProduct()
        {
            
        }

        public FoodProduct(string name, Category category)
        {
            ValidateFoodProductName(name);
            Name = UppercaseFirst(name.ToLower());
            Category = category;

            this.AddDomainEvent(new FoodProductAddedEvent(this));
        }

        public void UpdateFoodProduct(string newName, Category category)
        {
            this.UpdateProductName(newName);
            this.UpdateProductCategory(category);
        }

        public void UpdateProductName(string newName)
        {
            ValidateFoodProductName(newName);
            Name = UppercaseFirst(newName);
        }

        public void UpdateProductCategory(Category category)
        {
             ValidateFoodProductCategory(category);
             Category = category;
        }

        private void ValidateFoodProductName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidInputException("Product name can't be empty.", "InvalidFoodProductName");
        }

        private void ValidateFoodProductCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
                throw new InvalidInputException("Product category can't be empty.", "InvalidFoodProductCategory");
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
