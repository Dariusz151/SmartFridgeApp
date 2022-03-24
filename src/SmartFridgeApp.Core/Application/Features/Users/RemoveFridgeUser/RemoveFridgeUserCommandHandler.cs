using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
{
    public class RemoveFridgeUserCommandHandler : IRequestHandler<RemoveFridgeUserCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFridgeUserCommandHandler(IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveFridgeUserCommand request, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(request.FridgeId);

            fridge.RemoveUser(request.UserId);

            await _unitOfWork.CommitAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}
