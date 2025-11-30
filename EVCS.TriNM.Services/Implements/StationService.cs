using EVCS.TriNM.Repositories;
using EVCS.TriNM.Repositories.Models;
using EVCS.TriNM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVCS.TriNM.Services.Implements
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<StationTriNm>> GetAllStationsAsync()
        {
            var result = await _unitOfWork.StationTriNMRepository.GetAllAsync();
            return result?.ToList() ?? new List<StationTriNm>();
        }

        public async Task<StationTriNm?> GetStationByIdAsync(int id)
        {
            return await _unitOfWork.StationTriNMRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<StationTriNm>> GetActiveStationsAsync()
        {
            var result = await _unitOfWork.StationTriNMRepository.GetActiveStationsAsync();
            return result?.ToList() ?? new List<StationTriNm>();
        }

        public async Task<StationTriNm?> GetStationWithChargersAsync(int stationId)
        {
            return await _unitOfWork.StationTriNMRepository.GetStationWithChargersAsync(stationId);
        }

        public async Task<IEnumerable<StationTriNm>> GetStationsByLocationAsync(string location)
        {
            var result = await _unitOfWork.StationTriNMRepository.GetStationsByLocationAsync(location);
            return result?.ToList() ?? new List<StationTriNm>();
        }

        public async Task<IEnumerable<StationTriNm>> SearchStationsAsync(string? name, string? location, bool? isActive)
        {
            var result = await _unitOfWork.StationTriNMRepository.SearchStationsAsync(name, location, isActive);
            return result?.ToList() ?? new List<StationTriNm>();
        }

        public async Task<StationTriNm> CreateStationAsync(StationTriNm station)
        {
            ArgumentNullException.ThrowIfNull(station);

            station.CreatedDate = DateTime.UtcNow;
            station.IsActive = true;

            await _unitOfWork.StationTriNMRepository.AddAsync(station);
            await _unitOfWork.SaveChangesAsync();

            return station;
        }

        public async Task<StationTriNm?> UpdateStationAsync(StationTriNm station)
        {
            ArgumentNullException.ThrowIfNull(station);

            var existingStation = await _unitOfWork.StationTriNMRepository.GetByIdAsync(station.StationTriNmid);
            if (existingStation == null)
            {
                return null;
            }

            existingStation.StationTriNmcode = station.StationTriNmcode;
            existingStation.StationTriNmname = station.StationTriNmname;
            existingStation.Address = station.Address;
            existingStation.City = station.City;
            existingStation.Province = station.Province;
            existingStation.Latitude = station.Latitude;
            existingStation.Longitude = station.Longitude;
            existingStation.Capacity = station.Capacity;
            existingStation.Owner = station.Owner;
            existingStation.ContactPhone = station.ContactPhone;
            existingStation.ContactEmail = station.ContactEmail;
            existingStation.Description = station.Description;
            existingStation.ImageUrl = station.ImageUrl;
            existingStation.IsActive = station.IsActive;
            existingStation.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.StationTriNMRepository.UpdateAsync(existingStation);
            await _unitOfWork.SaveChangesAsync();

            return existingStation;
        }

        public async Task<bool> DeleteStationAsync(int id, bool hardDelete = false)
        {
            var station = await _unitOfWork.StationTriNMRepository.GetByIdAsync(id);
            if (station == null)
            {
                return false;
            }

            if (hardDelete)
            {
                await _unitOfWork.StationTriNMRepository.DeleteAsync(station);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                station.IsActive = false;
                station.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.StationTriNMRepository.UpdateAsync(station);
                await _unitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> ActivateStationAsync(int id)
        {
            var station = await _unitOfWork.StationTriNMRepository.GetByIdAsync(id);
            if (station == null)
            {
                return false;
            }

            if (station.IsActive)
            {
                return true;
            }

            station.IsActive = true;
            station.ModifiedDate = DateTime.UtcNow;
            await _unitOfWork.StationTriNMRepository.UpdateAsync(station);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
