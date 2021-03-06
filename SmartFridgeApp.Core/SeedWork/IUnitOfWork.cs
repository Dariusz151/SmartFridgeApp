﻿using System.Threading;
using System.Threading.Tasks;

namespace SmartFridgeApp.Core.SeedWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
