using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API.FoodProducts.GetFoodProducts
{
    public class GetFoodProductsQueryHandler : IRequestHandler<GetFoodProductsQuery, IEnumerable<FoodProduct>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFoodProductsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<FoodProduct>> Handle(GetFoodProductsQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[FoodProductId], " +
                               "[Name] " +
                               "FROM " +
                               "[dbo].[v_FoodProducts]";

            var foodProducts = await connection.QueryAsync<FoodProduct>(sql);

            return foodProducts.AsEnumerable();
        }
    }
}
