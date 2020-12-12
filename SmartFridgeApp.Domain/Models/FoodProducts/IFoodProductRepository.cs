﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Models.FoodProducts
{
    public interface IFoodProductRepository
    {
        Task<FoodProduct> GetByIdAsync(int foodProductId);
        Task<FoodProduct> GetByNameAsync(string foodProductName);
        Task AddAsync(FoodProduct foodProduct);
        Task<ICollection<FoodProduct>> GetAllAsync();
        Task DeleteAsync(int foodProductId);

        Task CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
