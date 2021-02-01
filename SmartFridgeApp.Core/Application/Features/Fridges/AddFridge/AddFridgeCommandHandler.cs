using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
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

            return new FridgeDto { Id = fridge.Id, Name = fridge.Name, Address = fridge.Address, Desc = fridge.Desc };
        }
    }
}
