using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EVCS.BlazorWebApp.TriNM.Services
{
    public class AuthenticationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(string token)
        {
            try
            {
                await _localStorage.SetItemAsync("authToken", token);
                ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var handler = new JwtSecurityTokenHandler();
                
                if (!handler.CanReadToken(token))
                {
                    Console.WriteLine("Invalid JWT token format");
                    await _localStorage.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var jwtToken = handler.ReadJwtToken(token);

                // Check if token is expired
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    Console.WriteLine("JWT token expired");
                    await _localStorage.RemoveItemAsync("authToken");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = jwtToken.Claims.ToList();
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting authentication state: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Remove invalid token
                try
                {
                    await _localStorage.RemoveItemAsync("authToken");
                }
                catch { }
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

