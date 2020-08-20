using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.SeedWork;
using Unit = MediatR.Unit;

namespace SmartFridgeApp.API.FridgeItems.ConsumeFridgeItem
{
    public class ConsumeFridgeItemCommandHandler : IRequestHandler<ConsumeFridgeItemCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConsumeFridgeItemCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ConsumeFridgeItemCommand command, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(command.FridgeId);
            var user = fridge.GetFridgeUser(command.UserId);
            
            user.ConsumeFridgeItem(command.FridgeItemId, command.AmountValue);
            
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
