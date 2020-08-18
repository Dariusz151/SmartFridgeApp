using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Fridges.AddFridge
{
    public class AddFridgeCommandHandler : IRequestHandler<AddFridgeCommand, FridgeDto>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFridgeCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FridgeDto> Handle(AddFridgeCommand command, CancellationToken cancellationToken)
        {
            var fridge = new Fridge(command.Name, command.Address, command.Desc);
            
            await _fridgeRepository.AddAsync(fridge);

            await _unitOfWork.CommitAsync(cancellationToken);

            Console.WriteLine("CommitAsync UnitOfWork.");

            return new FridgeDto { Id = fridge.Id, Name = fridge.Name };
        }
    }
}
