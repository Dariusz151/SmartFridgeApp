using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class FridgeUserService(
    IFridgeRepository fridgeRepository,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IFridgeUserService
{
    public async Task<IEnumerable<FridgeUserDto>> GetFridgeUsersAsync(Guid fridgeId, CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT u."Id", u."Name", u."Email"
            FROM app."Users" u
            WHERE u."FridgeId" = @FridgeId
            """;

        var users = await connection.QueryAsync<FridgeUserDto>(sql, new { FridgeId = fridgeId });
        return users.AsEnumerable();
    }

    public async Task AddFridgeUserAsync(Guid fridgeId, UserDto userDto, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = new User(userDto.Name, userDto.Email);
        fridge.AddUser(user);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task UpdateFridgeUserAsync(Guid userId, string name, Guid fridgeId, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        var user = fridge.GetFridgeUser(userId);
        user.UpdateUserName(name);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task RemoveFridgeUserAsync(Guid fridgeId, Guid userId, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        fridge.RemoveUser(userId);

        await unitOfWork.CommitAsync(ct);
    }
}
