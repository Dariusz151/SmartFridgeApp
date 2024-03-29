﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.SeedWork;
using Unit = MediatR.Unit;

namespace SmartFridgeApp.Core.Application.Features
{
    public class RemoveFridgeItemCommandHandler :IRequestHandler<RemoveFridgeItemCommand>
    {
        private readonly IFridgeRepository _fridgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFridgeItemCommandHandler(
            IFridgeRepository fridgeRepository, IUnitOfWork unitOfWork)
        {
            _fridgeRepository = fridgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemoveFridgeItemCommand command, CancellationToken cancellationToken)
        {
            var fridge = await _fridgeRepository.GetByIdAsync(command.FridgeId);
            var user = fridge.GetFridgeUser(command.UserId);
            user.RemoveFridgeItem(command.FridgeItemId);
            
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
