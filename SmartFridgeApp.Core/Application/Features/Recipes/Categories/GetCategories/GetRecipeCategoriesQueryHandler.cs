using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetRecipeCategoriesQueryHandler : IRequestHandler<GetRecipeCategoriesQuery, IEnumerable<RecipeCategory>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        //private readonly IRecipeRepository _recipeRepository;

        public GetRecipeCategoriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<RecipeCategory>> Handle(GetRecipeCategoriesQuery request, CancellationToken cancellationToken)
        {
            //return await _recipeRepository.GetAllRecipeCategoriesAsync();
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[RecipeCategoryId] as [RecipeCategoryId], " +
                               "[Name] as [Name] " +
                               "FROM " +
                               "[dbo].[v_RecipeCategories]";

            var categories = await connection.QueryAsync<RecipeCategory>(sql);

            return categories.AsEnumerable();
        }
    }
}
