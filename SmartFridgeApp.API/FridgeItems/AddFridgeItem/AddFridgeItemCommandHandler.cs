using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.FridgeItems;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Shared;
using Unit = MediatR.Unit;

namespace SmartFridgeApp.API.FridgeItems.AddFridgeItem
{
    public class AddFridgeItemCommandHandler : IRequestHandler<AddFridgeItemCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFridgeItemCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork, IFoodProductRepository foodProductRepository)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
            _foodProductRepository = foodProductRepository;
        }

        public async Task<Unit> Handle(AddFridgeItemCommand command, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(command.FridgeId);
            var user = fridge.GetFridgeUser(command.UserId);
            var foodProduct = await _foodProductRepository.GetByIdAsync(command.FridgeItemDto.FoodProductId);
            
            var fridgeItem = new FridgeItem(
                foodProduct,
                command.FridgeItemDto.Description,
                new AmountValue(
                    command.FridgeItemDto.Value,
                    command.FridgeItemDto.Unit)
            );

            fridgeItem.Category = command.FridgeItemDto.Category;
            // fridgeItem.ExpirationDate = 
            
            user.AddFridgeItem(fridgeItem);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
