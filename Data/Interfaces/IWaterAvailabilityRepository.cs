
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Interfaces
{
    public interface IWaterAvailabilityRepository
    {
        IEnumerable<WaterAvailability> WaterAvailabilities { get; }

        public Task<WaterAvailability> AddWaterAvailabilityAsync(WaterAvailability waterAvailability);

        Task<WaterAvailability> GetWaterAvailabilityAsync(int id);

        Task UpdateWaterAvailability(WaterAvailability updatedWaterAvailabilty);

        public Task DeleteWaterAvailabitly(WaterAvailability waterAvailability);
    }
}
