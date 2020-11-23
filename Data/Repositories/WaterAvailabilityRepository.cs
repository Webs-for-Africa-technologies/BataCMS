using BataCMS.Data;
using COHApp.Data.Interfaces;
using COHApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Repositories
{
    public class WaterAvailabilityRepository : IWaterAvailabilityRepository
    {
        private readonly AppDbContext _appDbContext;

        public WaterAvailabilityRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<WaterAvailability> WaterAvailabilities => _appDbContext.WaterAvailabilities;

        public async Task<WaterAvailability> AddWaterAvailabilityAsync(WaterAvailability waterAvailability)
        {
            await _appDbContext.WaterAvailabilities.AddAsync(waterAvailability);
            await _appDbContext.SaveChangesAsync();
            return waterAvailability;
        }

        public async Task DeleteWaterAvailabitly(WaterAvailability waterAvailability)
        {
            _appDbContext.WaterAvailabilities.Remove(waterAvailability);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<WaterAvailability> GetWaterAvailabilityAsync(int id)
        {
            return await _appDbContext.WaterAvailabilities.FirstOrDefaultAsync(p => p.WaterAvailabilityId == id);
        }

        public async Task UpdateWaterAvailability(WaterAvailability updatedWaterAvailabilty)
        {
            _appDbContext.WaterAvailabilities.Update(updatedWaterAvailabilty);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
