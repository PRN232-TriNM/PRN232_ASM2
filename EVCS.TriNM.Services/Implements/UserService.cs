using EVCS.TriNM.Repositories;
using EVCS.TriNM.Repositories.Models;
using EVCS.TriNM.Services.Interfaces;
using EVCS.TriNM.Services.Object.Requests;
using EVCS.TriNM.Services.Object.Responses;

namespace EVCS.TriNM.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<UserResponse>> GetUsersAsync()
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }

        public async Task<UserResponse?> GetMyProfileAsync(int userId)
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }

        public async Task<(bool isSuccess, string resultOrError)> UpdateUserProfileAsync(int id, UpdateUserProfileRequest request)
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }

        public async Task<UserResponse?> SoftDeleteUserAsync(int id)
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }

        public async Task<bool> UpgradeToPremiumAsync(int userId)
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }
    }
}
