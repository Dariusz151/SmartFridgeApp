using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.FoodProducts.AddFoodProduct
{
    public class AddFoodProductCommandHandler : IRequestHandler<AddFoodProductCommand>
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFoodProductCommandHandler(IFoodProductRepository foodProductRepository, IUnitOfWork unitOfWork)
        {
            _foodProductRepository = foodProductRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddFoodProductCommand command, CancellationToken cancellationToken)
        {
            var category = await _foodProductRepository.GetCategoryByIdAsync(command.CategoryId);

            var foodProduct = new FoodProduct(command.Name, category);

            await _foodProductRepository.AddAsync(foodProduct);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
