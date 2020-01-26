using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Users.UpdateFridgeUser
{
    public class UpdateFridgeUserCommandHandler : IRequestHandler<UpdateFridgeUserCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFridgeUserCommandHandler(IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFridgeUserCommand command, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(command.FridgeId);
            var user = fridge.GetFridgeUser(command.UserId);
            user.UpdateUser(command.Name);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
