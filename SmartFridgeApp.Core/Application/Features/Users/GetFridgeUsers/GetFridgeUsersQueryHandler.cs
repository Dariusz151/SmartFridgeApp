using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using SmartFridgeApp.Core.SeedWork;

namespace SmartFridgeApp.Core.Application.Features
{
    public class GetFridgeUsersQueryHandler : IRequestHandler<GetFridgeUsersQuery, IEnumerable<FridgeUserDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetFridgeUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<FridgeUserDto>> Handle(GetFridgeUsersQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                               "[Id], " +
                               "[Name], " +
                               "[Email] " +
                               "FROM [dbo].[v_FridgeUsers] " +
                               "WHERE [FridgeId] = @FridgeId";
            var users = await connection.QueryAsync<FridgeUserDto>(sql, new { request.FridgeId });

            return users.AsEnumerable();
        }
    }
}
