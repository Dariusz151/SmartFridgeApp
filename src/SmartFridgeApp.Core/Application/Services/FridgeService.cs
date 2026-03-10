using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class FridgeService(
    IFridgeRepository fridgeRepository,
    IUnitOfWork unitOfWork) : IFridgeService
{
    private const string FirstMemberColor = "#00695c";

    public async Task<IEnumerable<FridgeDto>> GetFridgesAsync(CancellationToken ct = default) =>
        await fridgeRepository.GetAllFridgesAsync();

    public async Task<FridgeDto> AddFridgeAsync(string name, string address, string desc, CancellationToken ct = default)
    {
        var fridge = new Fridge(name, address, desc);
        await fridgeRepository.AddAsync(fridge);
        await unitOfWork.CommitAsync(ct);

        return new FridgeDto { Id = fridge.Id, Name = fridge.Name, Address = fridge.Address, Desc = fridge.Desc, WasteScore = fridge.WasteScore, CreatedAt = fridge.CreatedAt };
    }

    /// <summary>
    /// Creates a fridge and adds the creator member in a single atomic transaction.
    /// EF knows the Fridge → FridgeMember relationship and orders INSERTs correctly.
    /// </summary>
    public async Task<FridgeDto> AddFridgeWithCreatorAsync(string name, string address, string desc, string creatorEmail, CancellationToken ct = default)
    {
        var fridge = new Fridge(name, address, desc);
        var creator = FridgeMember.CreateCreator(fridge.Id, creatorEmail, FirstMemberColor);
        fridge.AddMember(creator);

        await fridgeRepository.AddAsync(fridge);
        await unitOfWork.CommitAsync(ct);

        return new FridgeDto { Id = fridge.Id, Name = fridge.Name, Address = fridge.Address, Desc = fridge.Desc, WasteScore = fridge.WasteScore, CreatedAt = fridge.CreatedAt };
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
