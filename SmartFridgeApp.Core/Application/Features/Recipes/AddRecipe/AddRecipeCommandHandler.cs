using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Exceptions;
using SmartFridgeApp.Core.Extensions;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
{
    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Recipe>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddRecipeCommandHandler(
            IRecipeRepository recipeRepository, IUnitOfWork unitOfWork, IFoodProductRepository foodProductRepository)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
            _foodProductRepository = foodProductRepository;
        }

        public async Task<Recipe> Handle(AddRecipeCommand command, CancellationToken cancellationToken)
        {
            if (!await ValidateInsertedIndexes(command))
            {
                throw new DomainException("DomainException", "Some product id's does not exist in database.");
            }

            //
            // TODO: violate DRY principle ! Change it!
            //
            var allFoodProducts = await _foodProductRepository.GetAllAsync();

            List<FoodProductDetails> productsDetails = new List<FoodProductDetails>();
            foreach (var item in command.Products)
            {
                var foodProductName = allFoodProducts.SingleOrDefault(x => x.FoodProductId == item.FoodProductId).Name;
                FoodProductDetails fpd = new FoodProductDetails(item.FoodProductId, foodProductName, item.AmountValue, item.IsOptional);
                productsDetails.Add(fpd);
            }
            
            // TODO: throw Infrastrcutre Exception!! If category doesnt exist - 500 Internal Server Error.
            var recipeCategory = await _recipeRepository.GetRecipeCategoryByIdAsync(command.RecipeCategory);

            var recipe = new Recipe(
                command.Name,
                command.Description,
                recipeCategory,
                productsDetails,
                command.RequiredTime,
                command.LevelOfDifficulty);

            await _recipeRepository.AddRecipeAsync(recipe);
            await _unitOfWork.CommitAsync(cancellationToken);

            return recipe;
        }

        private async Task<bool> ValidateInsertedIndexes(AddRecipeCommand command)
        {
            var allFoodProducts = await _foodProductRepository.GetAllAsync();
            List<short> allFoodProductsId = allFoodProducts.Select(x => x.FoodProductId).ToList();
            List<short> insertedFoodProductsId = command.Products.Select(x => x.FoodProductId).ToList();

            return allFoodProductsId.ContainsAllItems(insertedFoodProductsId);
        }
    }
}
