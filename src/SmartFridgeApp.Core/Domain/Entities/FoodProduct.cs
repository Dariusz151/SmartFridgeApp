using System;
using System.Collections.Generic;
using SmartFridgeApp.Core.Application.Events;
using SmartFridgeApp.Core.Domain.ValueObjects;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Shared.Domain;

namespace SmartFridgeApp.Core.Domain.Entities
{
    public class FoodProduct
    {
        public short FoodProductId { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public DateTime InsertedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<ProductVariant> _variants = [];
        public IReadOnlyList<ProductVariant> Variants => _variants.AsReadOnly();

        private FoodProduct()
        {

        }

        public FoodProduct(string name, Category category)
        {
            ValidateFoodProductName(name);
            Name = UppercaseFirst(name.ToLower());
            Category = category;
            InsertedAt = DateTime.UtcNow;
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
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProductCategory(Category category)
        {
            ValidateFoodProductCategory(category);
            Category = category;
            UpdatedAt = DateTime.UtcNow;
        }

        public ProductVariant AddVariant(string name, string? barcode = null)
        {
            var variant = new ProductVariant(this, name, barcode);
            _variants.Add(variant);
            return variant;
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
