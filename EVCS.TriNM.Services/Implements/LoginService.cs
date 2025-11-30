using EVCS.TriNM.Repositories;
using EVCS.TriNM.Repositories.Models;
using EVCS.TriNM.Services.Extensions;
using EVCS.TriNM.Services.Interfaces;
using EVCS.TriNM.Services.Object.Requests;

namespace EVCS.TriNM.Services.Implements
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordEncryptionService _passwordEncryptionService;
        private readonly IAuthService _authService;

        public LoginService(IUnitOfWork unitOfWork, PasswordEncryptionService passwordEncryptionService, IAuthService authService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordEncryptionService = passwordEncryptionService ?? throw new ArgumentNullException(nameof(passwordEncryptionService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<(bool isSuccess, string resultOrError)> ValidateUserAsync(LoginRequest request)
        {
            throw new NotImplementedException("UserAccount entity is not available. Only StationTriNM and ChargerTriNM are supported.");
        }

        public async Task<UserAccount?> GetUserAccountAsync(string email, string password)
        {
            var user = await _unitOfWork.UserAccountRepository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return null;
            }

            var isValidPassword = _passwordEncryptionService.VerifyPassword(password, user.PasswordHash);
            if (!isValidPassword)
            {
                return null;
            }

            return user;
        }
    }
}
