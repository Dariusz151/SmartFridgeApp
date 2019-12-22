using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.Fridges.DeleteFridge
{
    public class DeleteFridgeCommandHandler : IRequestHandler<DeleteFridgeCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFridgeCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(DeleteFridgeCommand command, CancellationToken cancellationToken)
        {
            await _fridgeRepository.DeleteAsync(command.FridgeId);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
