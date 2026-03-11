using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFridgeService
{
    Task<IEnumerable<FridgeDto>> GetFridgesAsync(CancellationToken ct = default);
    Task<FridgeDto> AddFridgeAsync(string name, string address, string desc, CancellationToken ct = default);
    Task<FridgeDto> AddFridgeWithCreatorAsync(string name, string address, string desc, string creatorEmail, CancellationToken ct = default);
    Task UpdateFridgeAsync(Guid fridgeId, string name, string desc, CancellationToken ct = default);
    Task DeleteFridgeAsync(Guid fridgeId, CancellationToken ct = default);
}
