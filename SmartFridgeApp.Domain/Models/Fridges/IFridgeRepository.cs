using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Models.Fridges
{
    public interface IFridgeRepository
    {
        Task<Fridge> GetByIdAsync(int id);
        Task AddAsync(Fridge fridge);
        Task DeleteAsync(int fridgeId);
    }
}
