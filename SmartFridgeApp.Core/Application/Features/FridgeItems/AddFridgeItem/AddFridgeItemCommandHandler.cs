using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.SeedWork;
using Unit = MediatR.Unit;

namespace SmartFridgeApp.Core.Application.Features
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
                foodProduct.FoodProductId,
                command.FridgeItemDto.Note,
                new AmountValue(
                    command.FridgeItemDto.Value,
                    command.FridgeItemDto.Unit)
            );
            
            user.AddFridgeItem(fridgeItem);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
