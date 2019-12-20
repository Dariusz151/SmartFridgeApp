using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.API.Users.AddFridgeUser;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Domain.Users;

namespace SmartFridgeApp.API.Users.RemoveFridgeUser
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
