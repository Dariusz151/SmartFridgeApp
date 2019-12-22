using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
        private readonly IUnitOfWork _unitOfWork;

        public AddFridgeItemCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddFridgeItemCommand command, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(command.FridgeId);
            var user = fridge.GetFridgeUser(command.UserId);
            
            var fridgeItem = new FridgeItem(
                command.FridgeItemDto.Name,
                command.FridgeItemDto.Desc,
                new AmountValue(
                    command.FridgeItemDto.AmountValue.Value,
                    command.FridgeItemDto.AmountValue.Unit)
            );

            fridgeItem.Category = command.FridgeItemDto.Category;
            // fridgeItem.ExpirationDate = 
            
            user.AddFridgeItem(fridgeItem);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
