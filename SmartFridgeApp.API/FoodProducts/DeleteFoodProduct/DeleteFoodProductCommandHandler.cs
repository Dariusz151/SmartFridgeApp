using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.FoodProducts.DeleteFoodProduct
{
    public class DeleteFoodProductCommandHandler : IRequestHandler<DeleteFoodProductCommand>
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFoodProductCommandHandler(IFoodProductRepository foodProductRepository, IUnitOfWork unitOfWork)
        {
            _foodProductRepository = foodProductRepository;
            _unitOfWork = unitOfWork;
        }
       
        public async Task<Unit> Handle(DeleteFoodProductCommand command, CancellationToken cancellationToken)
        {
            await _foodProductRepository.DeleteAsync(command.FoodProductId);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
