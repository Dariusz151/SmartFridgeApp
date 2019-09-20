using SmartFridgeApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Repositories
{
    public interface IFoodItemRepository
    {
        Task<bool> AddFoodItem(FoodItem item, User user);
    }
}
