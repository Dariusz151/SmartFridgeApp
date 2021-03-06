﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using SmartFridgeApp.Core.Contracts.Repositories;
using SmartFridgeApp.Core.Domain.Entities;
using SmartFridgeApp.Core.Exceptions;
using System.Collections.Generic;
using SmartFridgeApp.Core.Application.Features;

namespace SmartFridgeApp.Infrastructure.Fridges
{
    public class FridgeRepository : IFridgeRepository
    {
        private readonly SmartFridgeAppContext _context;

        public Task<IEnumerable<FridgeDto>> GetAllFridgesAsync()
        {
            throw new NotImplementedException();
        }

        public FridgeRepository(SmartFridgeAppContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Fridge fridge)
        {
            await _context.Fridges.AddAsync(fridge);
        }
        
        public async Task DeleteAsync(Guid fridgeId)
        {
            var fridge = await GetByIdAsync(fridgeId);
            _context.Fridges.Remove(fridge);
        }


        public async Task<Fridge> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Fridges.Where(x=>x.Id == id).SingleAsync();
            }
            catch
            {
                throw new InvalidFridgeException("This fridge id does not exist.", "InvalidFridgeId");
            }
        }
    }
}
