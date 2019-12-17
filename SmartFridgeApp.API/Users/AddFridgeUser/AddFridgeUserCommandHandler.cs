using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.Users;

namespace SmartFridgeApp.API.Users.AddFridgeUser
{
    public class AddFridgeUserCommandHandler : IRequestHandler<AddFridgeUserCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;

        public AddFridgeUserCommandHandler(IFridgeRepository fridgeRepository)
        {
            _fridgeRepository = fridgeRepository;
        }
        
        public async Task<Unit> Handle(AddFridgeUserCommand request, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(request.FridgeId);

            var user = new User(request.User.Name, request.User.Email);
            var userId = user.Id;
            
            fridge.AddUser(user);

            return Unit.Value;
        }
    }
}
