using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Fridges.GetFridges
{
    public class GetFridgesQueryHandler : IRequestHandler<GetFridgesQuery, IEnumerable<FridgeDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFridgesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<FridgeDto>> Handle(GetFridgesQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                                    "[Id], " +
                                    "[Name], " +
                                    "[Address], " +
                                    "[Desc] " +
                               "FROM " +
                                    "[dbo].[v_Fridges]";

            var fridges = await connection.QueryAsync<FridgeDto>(sql);

            return fridges.AsEnumerable();
        }
    }
}
