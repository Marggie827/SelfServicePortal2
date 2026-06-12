using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;

namespace SelfServicePortal.Components.Pages.Auth
{
    public class UserSession
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private const string SessionKey = "UserSession";
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly ILogger<CustomAuthStateProvider>? _logger;
        private UserSession? _cachedSession;

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage, ILogger<CustomAuthStateProvider>? logger = null)
        {
            _sessionStorage = sessionStorage ?? throw new ArgumentNullException(nameof(sessionStorage));
            _logger = logger;
        }

        public async Task LoginAsync(UserSession session)
        {
            _cachedSession = session ?? throw new ArgumentNullException(nameof(session));
            try
            {
                await _sessionStorage.SetAsync(SessionKey, _cachedSession);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to persist user session");
            }

            var identity = CreateClaimsIdentity(_cachedSession);
            var principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task LogoutAsync()
        {
            _cachedSession = null;
            try
            {
                await _sessionStorage.DeleteAsync(SessionKey);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to remove user session");
            }

            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (_cachedSession == null)
                {
                    var result = await _sessionStorage.GetAsync<UserSession>(SessionKey);
                    if (result.Success && result.Value != null)
                    {
                        _cachedSession = result.Value;
                    }
                }

                if (_cachedSession != null)
                {
                    var identity = CreateClaimsIdentity(_cachedSession);
                    var principal = new ClaimsPrincipal(identity);
                    return new AuthenticationState(principal);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while reading authentication state");
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private ClaimsIdentity CreateClaimsIdentity(UserSession session)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString()),
                new Claim(ClaimTypes.Name, session.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, session.Email ?? string.Empty)
            };

            if (!string.IsNullOrWhiteSpace(session.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, session.Role));
            }

            return new ClaimsIdentity(claims, "CustomAuth");
        }
    }
}
