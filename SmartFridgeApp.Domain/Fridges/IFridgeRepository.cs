using System.Threading.Tasks;

namespace SmartFridgeApp.Domain.Fridges
{
    public interface IFridgeRepository
    {
        Task<Fridge> GetByIdAsync(FridgeId id);
        Task AddAsync(Fridge fridge);
    }
}
