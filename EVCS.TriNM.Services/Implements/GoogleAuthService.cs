using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using EVCS.TriNM.Repositories.Models;
using EVCS.TriNM.Repositories;
using EVCS.TriNM.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EVCS.TriNM.Services.Implements
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GoogleAuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration;
        }

        public async Task<string> ValidateGoogleTokenAndGetOrCreateUser(string credential)
        {
            try
            {
                var clientId = _configuration["Authentication:Google:ClientId"];
                if (string.IsNullOrEmpty(clientId))
                {
                    throw new InvalidOperationException("Google ClientId is not configured");
                }

                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

                var existingUser = await _unitOfWork.UserAccountRepository.GetByGoogleIdAsync(payload.Subject);
                UserAccount user;

                if (existingUser != null)
                {
                    user = existingUser;
                }
                else
                {
                    var existingEmail = await _unitOfWork.UserAccountRepository.GetByEmailAsync(payload.Email);
                    if (existingEmail != null)
                    {
                        existingEmail.GoogleId = payload.Subject;
                        existingEmail.PhotoUrl = payload.Picture;
                        await _unitOfWork.UserAccountRepository.UpdateAsync(existingEmail);
                        await _unitOfWork.SaveChangesAsync();
                        user = existingEmail;
                    }
                    else
                    {
                        user = new UserAccount
                        {
                            UserName = payload.Email.Split('@')[0],
                            Email = payload.Email,
                            FullName = payload.Name,
                            GoogleId = payload.Subject,
                            PhotoUrl = payload.Picture,
                            RoleId = 2,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow
                        };

                        await _unitOfWork.UserAccountRepository.AddAsync(user);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                var authService = new AuthService(_unitOfWork, _configuration);
                return authService.GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Google authentication failed: {ex.Message}", ex);
            }
        }
    }
}
