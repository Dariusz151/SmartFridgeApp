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

public class FridgeService(
    IFridgeRepository fridgeRepository,
    IUnitOfWork unitOfWork,
    ISqlConnectionFactory sqlConnectionFactory) : IFridgeService
{
    public async Task<IEnumerable<FridgeDto>> GetFridgesAsync(CancellationToken ct = default)
    {
        var connection = sqlConnectionFactory.GetOpenConnection();
        const string sql = """
            SELECT "Id", "Name", "Address", "Desc"
            FROM app."Fridges"
            """;

        var fridges = await connection.QueryAsync<FridgeDto>(sql);
        return fridges.AsEnumerable();
    }

    public async Task<FridgeDto> AddFridgeAsync(string name, string address, string desc, CancellationToken ct = default)
    {
        var fridge = new Fridge(name, address, desc);
        await fridgeRepository.AddAsync(fridge);
        await unitOfWork.CommitAsync(ct);

        return new FridgeDto { Id = fridge.Id, Name = fridge.Name, Address = fridge.Address, Desc = fridge.Desc };
    }

    public async Task UpdateFridgeAsync(Guid fridgeId, string name, string desc, CancellationToken ct = default)
    {
        var fridge = await fridgeRepository.GetByIdAsync(fridgeId);
        fridge.ChangeFridgeName(name);
        fridge.ChangeFridgeDesc(desc);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task DeleteFridgeAsync(Guid fridgeId, CancellationToken ct = default)
    {
        await fridgeRepository.DeleteAsync(fridgeId);
        await unitOfWork.CommitAsync(ct);
    }
}
