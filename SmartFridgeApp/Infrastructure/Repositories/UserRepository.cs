using SmartFridgeApp.Domain.Models;
using SmartFridgeApp.Domain.Repositories;
using SmartFridgeApp.Persistence;
using System;
using System.Threading.Tasks;

namespace SmartFridgeApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartFridgeContext _context;

        public UserRepository(SmartFridgeContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            User user = null;

            user = await _context.Users.FindAsync(userId);
            if (user is null)
                return null;

            return user;
        }
    }
}
