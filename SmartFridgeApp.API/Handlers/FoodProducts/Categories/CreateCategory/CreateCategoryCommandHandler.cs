using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.FoodProducts.Categories.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand>
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IFoodProductRepository foodProductRepository, IUnitOfWork unitOfWork)
        {
            _foodProductRepository = foodProductRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = new Category(command.Name);

            await _foodProductRepository.CreateCategoryAsync(category);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
