using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Fridges.UpdateFridge
{
    public class UpdateFridgeCommandHandler : IRequestHandler<UpdateFridgeCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFridgeCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFridgeCommand request, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(request.FridgeId);

            fridge.ChangeFridgeName(request.Name);
            fridge.ChangeFridgeDesc(request.Desc);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
