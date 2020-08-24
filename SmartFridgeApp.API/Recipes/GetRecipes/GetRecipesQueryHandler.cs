using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.Extensions;

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
           
            //const string query = @"
            //     SELECT 
	           //     r.RecipeId as [RecipeId],
	           //     r.Name as [RecipeName],
            //        r.Description as [Description],
            //        r.DifficultyLevel as [DifficultyLevel],
            //        r.MinutesRequired as [MinutesRequired],
            //        r.Category as [Category],
            //        r.FoodProducts as [FoodProducts]
            //     FROM [dbo].[v_Recipes] r
            //    ";

            const string query = @"
                 SELECT 
	                r.RecipeId as [RecipeId],
	                r.Name as [RecipeName],
                    r.Description as [Description],
                    r.RecipeCategory as [RecipeCategory],
                    r.FoodProducts as [FoodProducts]
                 FROM [dbo].[v_Recipes] r
                ";

            var recipes = await connection.QueryAsync<RecipeDto>(query);
            foreach (RecipeDto recipe in (IList)recipes)
            {
                recipe.FoodProducts = ConvertHelper.ConvertXmlToJson(recipe.FoodProducts);
            }
            
            return recipes;
        }
    }
}
