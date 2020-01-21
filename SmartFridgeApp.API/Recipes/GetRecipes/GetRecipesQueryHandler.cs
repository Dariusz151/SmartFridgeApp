using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.API.FoodProducts;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API.Recipes.GetRecipes
{
    public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, IEnumerable<RecipeDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRecipesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<RecipeDto>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
           
            const string query = @"
                 SELECT 
	                r.RecipeId as [RecipeId],
	                r.Name as [RecipeName],
                    r.Description,
                    r.DifficultyLevel,
                    r.MinutesRequired,
                    r.Category,
	                rfp.FoodProductId as [FoodProductId],
	                fp.Name as [FoodProductName]
                 FROM [app].[Recipes] r
                 LEFT JOIN [dbo].RecipeFoodProduct rfp
	                ON r.RecipeId = rfp.RecipeId
                 LEFT JOIN [app].FoodProducts fp
	                ON fp.FoodProductId = rfp.FoodProductId
                ";

            var recipesDict = new Dictionary<int, RecipeDto>();
            var list = connection.Query<RecipeDto, FoodProductDto, RecipeDto>(
                    query, (recipeDto, foodProductDto) =>
                    {
                        if (!recipesDict.TryGetValue(recipeDto.RecipeId, out var recipe))
                        {
                            recipe = recipeDto;
                            recipe.FoodProducts = new List<FoodProductDto>();
                            recipesDict.Add(recipe.RecipeId, recipe);
                        }

                        recipe.FoodProducts.Add(foodProductDto);
                        return recipe;
                    }, splitOn: "FoodProductId")
                .Distinct().ToList();
            
            return list.AsEnumerable();
        }
    }
}
