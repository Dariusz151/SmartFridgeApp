using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API.FoodProducts.Categories.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetCategoriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[CategoryId] as [CategoryId], " +
                               "[Name] as [Name]" +
                               "FROM " +
                               "[app].[Categories]";

            var categories = await connection.QueryAsync<Category>(sql);

            return categories.AsEnumerable();
        }
    }
}
