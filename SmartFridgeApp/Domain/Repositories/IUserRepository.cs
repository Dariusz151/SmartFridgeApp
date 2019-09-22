using SmartFridgeApp.Domain.Models;
using System;
using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
    }
}
