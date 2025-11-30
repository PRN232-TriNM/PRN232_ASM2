using EVCS.TriNM.Repositories.Models;
using EVCS.TriNM.Services.Implements;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using HotChocolate;

namespace EVCS.GraphQLWebAPI.TriNM.GraphQL
{
    public class Queries
    {
        private readonly IServiceProviders _serviceProvider;

        public Queries(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [GraphQLName("getStations")]
        public async Task<IEnumerable<StationTriNm>> GetStations()
        {
            try
            {
                var result = await _serviceProvider.StationService.GetAllStationsAsync();
                return result?.ToList() ?? new List<StationTriNm>();
            }
            catch (Exception ex)
            {
                return new List<StationTriNm>();
            }
        }

        [GraphQLName("getStationById")]
        public async Task<StationTriNm?> GetStationById(int id)
        {
            try
            {
                return await _serviceProvider.StationService.GetStationByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }

        [GraphQLName("getStationsByLocation")]
        public async Task<IEnumerable<StationTriNm>> GetStationsByLocation(string location)
        {
            try
            {
                return await _serviceProvider.StationService.GetStationsByLocationAsync(location) ?? new List<StationTriNm>();
            }
            catch
            {
                return new List<StationTriNm>();
            }
        }

        [GraphQLName("getStationWithChargers")]
        public async Task<StationTriNm?> GetStationWithChargers(int stationId)
        {
            try
            {
                return await _serviceProvider.StationService.GetStationWithChargersAsync(stationId);
            }
            catch
            {
                return null;
            }
        }

        [GraphQLName("getActiveStations")]
        public async Task<IEnumerable<StationTriNm>> GetActiveStations()
        {
            try
            {
                return await _serviceProvider.StationService.GetActiveStationsAsync() ?? new List<StationTriNm>();
            }
            catch
            {
                return new List<StationTriNm>();
            }
        }

        [GraphQLName("searchStations")]
        public async Task<IEnumerable<StationTriNm>> SearchStations(string? name, string? location, bool? isActive)
        {
            try
            {
                return await _serviceProvider.StationService.SearchStationsAsync(name, location, isActive) ?? new List<StationTriNm>();
            }
            catch
            {
                return new List<StationTriNm>();
            }
        }

        [GraphQLName("getChargers")]
        public async Task<IEnumerable<ChargerTriNm>> GetChargers()
        {
            try
            {
                var result = await _serviceProvider.ChargerService.GetAllChargersAsync();
                return result?.ToList() ?? new List<ChargerTriNm>();
            }
            catch
            {
                return new List<ChargerTriNm>();
            }
        }

        [GraphQLName("getChargerById")]
        public async Task<ChargerTriNm?> GetChargerById(int id)
        {
            try
            {
                return await _serviceProvider.ChargerService.GetChargerByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }

        [GraphQLName("getChargersByStationId")]
        public async Task<IEnumerable<ChargerTriNm>> GetChargersByStationId(int stationId)
        {
            try
            {
                return await _serviceProvider.ChargerService.GetChargersByStationIdAsync(stationId) ?? new List<ChargerTriNm>();
            }
            catch
            {
                return new List<ChargerTriNm>();
            }
        }

        [GraphQLName("getAvailableChargers")]
        public async Task<IEnumerable<ChargerTriNm>> GetAvailableChargers()
        {
            try
            {
                return await _serviceProvider.ChargerService.GetAvailableChargersAsync() ?? new List<ChargerTriNm>();
            }
            catch
            {
                return new List<ChargerTriNm>();
            }
        }
    }
}
