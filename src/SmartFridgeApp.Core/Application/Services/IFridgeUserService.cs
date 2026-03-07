using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Core.Application.Services;

public interface IFridgeUserService
{
    Task<IEnumerable<FridgeUserDto>> GetFridgeUsersAsync(Guid fridgeId, CancellationToken ct = default);
    Task AddFridgeUserAsync(Guid fridgeId, UserDto user, CancellationToken ct = default);
    Task UpdateFridgeUserAsync(Guid userId, string name, Guid fridgeId, CancellationToken ct = default);
    Task RemoveFridgeUserAsync(Guid fridgeId, Guid userId, CancellationToken ct = default);
}
