using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;

namespace SmartFridgeApp.API.FoodProducts.UpdateFoodProduct
{
    public class UpdateFoodProductCommandHandler : IRequestHandler<UpdateFoodProductCommand>
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFoodProductCommandHandler(IFoodProductRepository foodProductRepository, IUnitOfWork unitOfWork)
        {
            _foodProductRepository = foodProductRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateFoodProductCommand command, CancellationToken cancellationToken)
        {
            var foodProduct = await _foodProductRepository.GetByIdAsync(command.FoodProductId);
            foodProduct.UpdateProductName(command.FoodProductName);
            
            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
