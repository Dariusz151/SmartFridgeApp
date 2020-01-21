using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.API.FoodProducts;
using SmartFridgeApp.API.Fridges;
using SmartFridgeApp.Domain.Recipes;
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
            //const string sql = "SELECT " +
            //                   "[RecipeId], " +
            //                   "[RecipeName], " +
            //                   "[FoodProductId], " +
            //                   "[FoodProductName] " +
            //                   "FROM " +
            //                   "[dbo].[v_Recipes]";

            var lookup = new Dictionary<int, RecipeDto>();
            connection.Query<RecipeDto, FoodProductDto, RecipeDto>(@"
                SELECT r.*, rfp.*
                FROM [Recipes] r
                INNER JOIN [RecipeFoodProducts] rfp
                    ON r.RecipeId = rfp.RecipeId ", (r, rfp) => {
                RecipeDto recipeDto;
                if (!lookup.TryGetValue(r.RecipeId, out recipeDto))
                    lookup.Add(r.RecipeId, recipeDto = r);
                if (recipeDto.RecipeFoodProducts == null)
                    recipeDto.RecipeFoodProducts = new List<FoodProductDto>();
                recipeDto.RecipeFoodProducts.Add(rfp);
                return recipeDto;
            }).AsQueryable();
            var resultList = lookup.Values;
            
            //var recipes = await connection.QueryAsync<RecipeDto>(sql);

            return resultList.AsEnumerable();
        }

    }
}
