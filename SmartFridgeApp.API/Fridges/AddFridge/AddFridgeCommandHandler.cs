using MediatR;
using SmartFridgeApp.Domain.Fridges;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFridgeApp.API.Fridges.AddFridge
{
    public class AddFridgeCommandHandler : IRequestHandler<AddFridgeCommand, FridgeDto>
    {
        private readonly IFridgeRepository _fridgeRepository;

        public AddFridgeCommandHandler(
            IFridgeRepository fridgeRepository)
        {
            _fridgeRepository = fridgeRepository;
        }

        public async Task<FridgeDto> Handle(AddFridgeCommand command, CancellationToken cancellationToken)
        {
            var fridge = new Fridge(command.Name, command.Address, command.Desc);
            
            await _fridgeRepository.AddAsync(fridge);
            
            return new FridgeDto { Id = fridge.Id.Value };
        }
    }
}
