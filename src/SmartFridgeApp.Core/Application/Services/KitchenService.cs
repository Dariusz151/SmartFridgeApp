using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Shared.SeedWork;

namespace SmartFridgeApp.Core.Application.Services;

public class KitchenService(
    IKitchenRepository KitchenRepository,
    IUnitOfWork unitOfWork) : IKitchenService
{
    private const string FirstMemberColor = "#00695c";

    public async Task<IEnumerable<KitchenDto>> GetKitchensAsync(CancellationToken ct = default) =>
        await KitchenRepository.GetAllFridgesAsync();

    public async Task<KitchenDto> AddKitchenAsync(string name, string address, string desc, CancellationToken ct = default)
    {
        var Kitchen = new Kitchen(name, address, desc);
        await KitchenRepository.AddAsync(Kitchen);
        await unitOfWork.CommitAsync(ct);

        return new KitchenDto { Id = Kitchen.Id, Name = Kitchen.Name, Address = Kitchen.Address, Desc = Kitchen.Desc, CreatedAt = Kitchen.CreatedAt };
    }

    public async Task<KitchenDto> AddKitchenWithCreatorAsync(string name, string address, string desc, string creatorEmail, CancellationToken ct = default)
    {
        var Kitchen = new Kitchen(name, address, desc);
        var creator = KitchenMember.CreateCreator(Kitchen.Id, creatorEmail, FirstMemberColor);
        Kitchen.AddMember(creator);

        await KitchenRepository.AddAsync(Kitchen);
        await unitOfWork.CommitAsync(ct);

        return new KitchenDto { Id = Kitchen.Id, Name = Kitchen.Name, Address = Kitchen.Address, Desc = Kitchen.Desc, CreatedAt = Kitchen.CreatedAt };
    }

    public async Task UpdateKitchenAsync(Guid kitchenId, string name, string desc, CancellationToken ct = default)
    {
        var Kitchen = await KitchenRepository.GetByIdAsync(kitchenId);
        Kitchen.ChangeKitchenName(name);
        Kitchen.ChangeKitchenDesc(desc);

        await unitOfWork.CommitAsync(ct);
    }

    public async Task DeleteKitchenAsync(Guid kitchenId, CancellationToken ct = default)
    {
        await KitchenRepository.DeleteAsync(kitchenId);
        await unitOfWork.CommitAsync(ct);
    }
}
