using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.API.Users;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API.FridgeItems.GetFridgeItems
{
    public class GetFridgeItemsQueryHandler : IRequestHandler<GetFridgeItemsQuery, IEnumerable<FridgeItemDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFridgeItemsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<FridgeItemDto>> Handle(GetFridgeItemsQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[FridgeItems].[ProductName], " +
                               "[FridgeItems].[FoodProductId], " +
                               "[FridgeItems].[Description], " +
                               "[FridgeItems].[Category], " +
                               "[FridgeItems].[EnteredAt], " +
                               "[FridgeItems].[Value], " +
                               "[FridgeItems].[Unit] " +
                               "FROM [dbo].[v_FridgeItems] AS [FridgeItems] " +
                               "WHERE [FridgeItems].[FridgeId] = @FridgeId " +
                               "AND [FridgeItems].[UserId] = @UserId";
            var fridgeItems = await connection.QueryAsync<FridgeItemDto>(sql, new { request.FridgeId, request.UserId });

            //return new List<FridgeItemDto>();

            return fridgeItems.AsEnumerable();
        }
    }
}
