using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Core.Application.Services;

public interface IKitchenService
{
    Task<IEnumerable<KitchenDto>> GetKitchensAsync(CancellationToken ct = default);
    Task<KitchenDto> AddKitchenAsync(string name, string address, string desc, CancellationToken ct = default);
    Task<KitchenDto> AddKitchenWithCreatorAsync(string name, string address, string desc, string creatorEmail, CancellationToken ct = default);
    Task UpdateKitchenAsync(Guid kitchenId, string name, string desc, CancellationToken ct = default);
    Task DeleteKitchenAsync(Guid kitchenId, CancellationToken ct = default);
}
