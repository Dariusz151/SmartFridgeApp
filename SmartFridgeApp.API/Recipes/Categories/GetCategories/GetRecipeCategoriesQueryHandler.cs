using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Recipes.Categories.GetCategories
{
    public class GetRecipeCategoriesQueryHandler : IRequestHandler<GetRecipeCategoriesQuery, IEnumerable<RecipeCategory>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRecipeCategoriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<RecipeCategory>> Handle(GetRecipeCategoriesQuery request, CancellationToken cancellationToken)
        {
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
