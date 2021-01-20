using Dapper;
using MediatR;
using SmartFridgeApp.Infrastructure.SeedWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFridgeApp.API.FridgeItems.GetFridgeItems
{
    public class GetFridgeItemsByIdQueryHandler : IRequestHandler<GetFridgeItemsByIdQuery, IEnumerable<FridgeItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFridgeItemsByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<FridgeItemDto>> Handle(GetFridgeItemsByIdQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[FridgeItems].[Id] as [FridgeItemId], " +
                               "[FridgeItems].[ProductName], " +
                               "[FridgeItems].[FoodProductId], " +
                               "[FridgeItems].[CategoryName], " +
                               "[FridgeItems].[CategoryId], " +
                               "[FridgeItems].[EnteredAt], " +
                               "[FridgeItems].[Value], " +
                               "[FridgeItems].[Unit] " +
                               "FROM [dbo].[v_FridgeItems] AS [FridgeItems] " +
                               "WHERE [FridgeItems].[FridgeId] = @FridgeId ";
            var fridgeItems = await connection.QueryAsync<FridgeItemDto>(sql, new { request.FridgeId });
            //  var fridgeItems = await connection.QueryAsync<FridgeItemDto>(sql, new { request.FridgeId, request.UserId });

            //return new List<FridgeItemDto>();

            return fridgeItems.AsEnumerable();
        }
    }

}
