using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Users;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Users.AddFridgeUser
{
    public class AddFridgeUserCommandHandler : IRequestHandler<AddFridgeUserCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFridgeUserCommandHandler(IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Unit> Handle(AddFridgeUserCommand request, CancellationToken cancellationToken)
        { 
            var fridge = await _fridgeRepository.GetByIdAsync(request.FridgeId);

            var user = new User(request.User.Name, request.User.Email);


            fridge.AddUser(user);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
