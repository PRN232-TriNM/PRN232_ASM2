using EVCS.TriNM.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVCS.TriNM.Services.Interfaces
{
    public interface IStationService
    {
        Task<IEnumerable<StationTriNm>> GetAllStationsAsync();
        Task<StationTriNm?> GetStationByIdAsync(int id);
        Task<IEnumerable<StationTriNm>> GetActiveStationsAsync();
        Task<StationTriNm?> GetStationWithChargersAsync(int stationId);
        Task<IEnumerable<StationTriNm>> GetStationsByLocationAsync(string location);
        Task<IEnumerable<StationTriNm>> SearchStationsAsync(string? name, string? location, bool? isActive);
        Task<StationTriNm> CreateStationAsync(StationTriNm station);
        Task<StationTriNm?> UpdateStationAsync(StationTriNm station);
        Task<bool> DeleteStationAsync(int id, bool hardDelete = false);
        Task<bool> ActivateStationAsync(int id);
    }
}
