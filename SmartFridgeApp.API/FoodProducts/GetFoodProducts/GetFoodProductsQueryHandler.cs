using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API.FoodProducts.GetFoodProducts
{
    public class GetFoodProductsQueryHandler : IRequestHandler<GetFoodProductsQuery, IEnumerable<FoodProductDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFoodProductsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<FoodProductDto>> Handle(GetFoodProductsQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[FoodProductId], " +
                               "[Name] as [FoodProductName], " +
                               "[Category] as [FoodProductCategory] " +
                               "FROM " +
                               "[dbo].[v_FoodProducts]";

            var foodProducts = await connection.QueryAsync<FoodProductDto>(sql);

            return foodProducts.AsEnumerable();
        }
    }
}
