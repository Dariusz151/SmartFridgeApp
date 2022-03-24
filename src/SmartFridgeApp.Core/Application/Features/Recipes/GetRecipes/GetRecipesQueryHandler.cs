using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Core.Domain.Shared;
using SmartFridgeApp.Core.Extensions;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
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
                    r.Description as [Description],
                    r.RecipeCategory as [RecipeCategory],
                    r.FoodProducts as [FoodProducts],
                    r.RequiredTime as [RequiredTime],
                    r.LevelOfDifficulty as [LevelOfDifficultyId]
                 FROM [dbo].[v_Recipes] r
                ";

            var recipes = await connection.QueryAsync<RecipeDto>(query);
            foreach (RecipeDto recipe in (IList)recipes)
            {
                recipe.FoodProducts = ConvertHelper.ConvertXmlToJson(recipe.FoodProducts);
                recipe.LevelOfDifficulty = Enum.GetName(typeof(LevelOfDifficulty), recipe.LevelOfDifficultyId);
            }

            return recipes;
        }
    }
}
