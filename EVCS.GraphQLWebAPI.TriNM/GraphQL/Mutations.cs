using EVCS.TriNM.Repositories.Models;
using EVCS.TriNM.Services.Implements;
using EVCS.GraphQLWebAPI.TriNM.GraphQL.Inputs;
using Microsoft.AspNetCore.Authorization;
using HotChocolate;
using Microsoft.AspNetCore.SignalR;
using EVCS.GraphQLWebAPI.TriNM.Hubs;

namespace EVCS.GraphQLWebAPI.TriNM.GraphQL
{
    public class Mutations
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly IHubContext<StationHub> _hubContext;

        public Mutations(IServiceProviders serviceProvider, IHubContext<StationHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        public async Task<StationTriNm> CreateStation(CreateStationInput input)
        {
            var station = new StationTriNm
            {
                StationTriNmcode = input.StationCode,
                StationTriNmname = input.StationName,
                Address = input.Address,
                City = input.City,
                Province = input.Province,
                Latitude = input.Latitude,
                Longitude = input.Longitude,
                Capacity = input.Capacity,
                CurrentAvailable = input.Capacity,
                Owner = input.Owner,
                ContactPhone = input.ContactPhone,
                ContactEmail = input.ContactEmail,
                Description = input.Description,
                ImageUrl = input.ImageURL,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            var result = await _serviceProvider.StationService.CreateStationAsync(station);
            await _hubContext.Clients.All.SendAsync("StationCreated", result);
            return result;
        }

        public async Task<StationTriNm?> UpdateStation(UpdateStationInput input)
        {
            var existingStation = await _serviceProvider.StationService.GetStationByIdAsync(input.StationId);
            if (existingStation == null)
            {
                throw new Exception($"Station with ID {input.StationId} not found");
            }

            if (input.StationCode != null)
                existingStation.StationTriNmcode = input.StationCode;
            if (input.StationName != null)
                existingStation.StationTriNmname = input.StationName;
            if (input.Address != null)
                existingStation.Address = input.Address;
            if (input.Owner != null)
                existingStation.Owner = input.Owner;
            if (input.City != null)
                existingStation.City = input.City;
            if (input.Province != null)
                existingStation.Province = input.Province;
            if (input.Latitude.HasValue)
                existingStation.Latitude = input.Latitude;
            if (input.Longitude.HasValue)
                existingStation.Longitude = input.Longitude;
            if (input.Capacity.HasValue)
                existingStation.Capacity = input.Capacity.Value;
            if (input.ContactPhone != null)
                existingStation.ContactPhone = input.ContactPhone;
            if (input.ContactEmail != null)
                existingStation.ContactEmail = input.ContactEmail;
            if (input.Description != null)
                existingStation.Description = input.Description;
            if (input.ImageURL != null)
                existingStation.ImageUrl = input.ImageURL;
            if (input.IsActive.HasValue)
                existingStation.IsActive = input.IsActive.Value;

            var result = await _serviceProvider.StationService.UpdateStationAsync(existingStation);
            if (result != null)
            {
                await _hubContext.Clients.All.SendAsync("StationUpdated", result);
            }
            return result;
        }

        public async Task<bool> DeleteStation(int id)
        {
            try
            {
                var result = await _serviceProvider.StationService.DeleteStationAsync(id, hardDelete: true);
                if (result)
                {
                    await _hubContext.Clients.All.SendAsync("StationDeleted", id);
                }
                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<LoginPayload> Login(LoginInput input)
        {
            try
            {
                var user = await _serviceProvider.LoginService.GetUserAccountAsync(input.Email, input.Password);
                if (user == null)
                {
                    return new LoginPayload { Token = null, Error = "Invalid email or password" };
                }

                if (!user.IsActive)
                {
                    return new LoginPayload { Token = null, Error = "Account is inactive" };
                }

                var token = _serviceProvider.AuthService.GenerateJwtToken(user);
                return new LoginPayload { Token = token, Error = null };
            }
            catch (Exception ex)
            {
                return new LoginPayload { Token = null, Error = "Server error: " + ex.Message };
            }
        }

        public async Task<LoginPayload> GoogleLogin(string credential)
        {
            try
            {
                var token = await _serviceProvider.GoogleAuthService.ValidateGoogleTokenAndGetOrCreateUser(credential);
                return new LoginPayload { Token = token, Error = null };
            }
            catch (Exception ex)
            {
                return new LoginPayload { Token = null, Error = "Google login failed: " + ex.Message };
            }
        }

        public async Task<LoginPayload> Register(RegisterInput input)
        {
            try
            {
                var request = new EVCS.TriNM.Services.Object.Requests.RegisterRequest
                {
                    UserName = input.UserName,
                    Email = input.Email,
                    Password = input.Password,
                    FullName = input.FullName,
                    Phone = input.Phone,
                    EmployeeCode = input.EmployeeCode,
                    RoleId = input.RoleId
                };

                var (isSuccess, resultOrError) = await _serviceProvider.RegisterService.RegisterUserAsync(request);
                if (!isSuccess)
                {
                    return new LoginPayload { Token = null, Error = resultOrError };
                }

                var user = await _serviceProvider.LoginService.GetUserAccountAsync(input.Email, input.Password);
                if (user == null)
                {
                    return new LoginPayload { Token = null, Error = "Registration successful but login failed" };
                }

                var token = _serviceProvider.AuthService.GenerateJwtToken(user);
                return new LoginPayload { Token = token, Error = null };
            }
            catch (Exception ex)
            {
                return new LoginPayload { Token = null, Error = "Registration failed: " + ex.Message };
            }
        }

        public async Task<ChargerTriNm> CreateCharger(CreateChargerInput input)
        {
            var charger = new ChargerTriNm
            {
                StationTriNmid = input.StationTriNMId,
                ChargerTriNmtype = input.ChargerTriNMType,
                IsAvailable = input.IsAvailable,
                ImageUrl = input.ImageURL
            };

            return await _serviceProvider.ChargerService.CreateChargerAsync(charger);
        }

        public async Task<bool> UpdateCharger(UpdateChargerInput input)
        {
            var existingCharger = await _serviceProvider.ChargerService.GetChargerByIdAsync(input.ChargerTriNMId);
            if (existingCharger == null)
            {
                throw new Exception($"Charger with ID {input.ChargerTriNMId} not found");
            }

            if (input.StationTriNMId.HasValue)
                existingCharger.StationTriNmid = input.StationTriNMId.Value;
            if (input.ChargerTriNMType != null)
                existingCharger.ChargerTriNmtype = input.ChargerTriNMType;
            if (input.IsAvailable.HasValue)
                existingCharger.IsAvailable = input.IsAvailable.Value;
            if (input.ImageURL != null)
                existingCharger.ImageUrl = input.ImageURL;

            return await _serviceProvider.ChargerService.UpdateChargerAsync(existingCharger);
        }

        public async Task<bool> DeleteCharger(int id)
        {
            try
            {
                return await _serviceProvider.ChargerService.DeleteChargerAsync(id);
            }
            catch
            {
                return false;
            }
        }
    }
}
