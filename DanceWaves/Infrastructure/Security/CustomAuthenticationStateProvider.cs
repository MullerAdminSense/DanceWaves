using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Serilog;

namespace DanceWaves.Infrastructure.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        // Cache tokens by circuit ID (Blazor Server session)
        private static readonly ConcurrentDictionary<string, string> _tokenCache = new();

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private string GetCircuitId()
        {
            // Try to get circuit ID from JS context
            try
            {
                // In Blazor Server, we can use a simple identifier
                // For now, use a static key - in production, use actual circuit ID
                return "default";
            }
            catch
            {
                return "default";
            }
        }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = null;
            var circuitId = GetCircuitId();
            
            // First, try to get from cache
            if (_tokenCache.TryGetValue(circuitId, out var cachedToken))
            {
                token = cachedToken;
            }
            
            // Then try to get from localStorage
            try
            {
                var tokenFromStorage = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");
                if (!string.IsNullOrWhiteSpace(tokenFromStorage))
                {
                    token = tokenFromStorage;
                    // Update cache
                    _tokenCache.AddOrUpdate(circuitId, token, (key, oldValue) => token);
                }
                else if (string.IsNullOrWhiteSpace(token))
                {
                    // No token in storage and no cache - clear cache
                    _tokenCache.TryRemove(circuitId, out _);
                }
            }
            catch (JSException ex)
            {
                Log.Error(ex, "JSException in GetAuthenticationStateAsync");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "InvalidOperationException in GetAuthenticationStateAsync");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unknown error in GetAuthenticationStateAsync");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                Log.Warning("No token found in GetAuthenticationStateAsync");
                return new AuthenticationState(_anonymous);
            }

            var principal = JwtTokenParser.ParseToken(token);
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Log.Warning("Invalid token in GetAuthenticationStateAsync");
                _tokenCache.TryRemove(circuitId, out _);
                return new AuthenticationState(_anonymous);
            }

            Log.Information("Authenticated user: {Name}", principal.Identity.Name);
            return new AuthenticationState(principal);
        }

    public async Task MarkUserAsAuthenticated(string accessToken)
        {
            var circuitId = GetCircuitId();
            
            // Save to cache immediately
            _tokenCache.AddOrUpdate(circuitId, accessToken, (key, oldValue) => accessToken);
            
            // Save to localStorage
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", accessToken);
            }
            catch
            {
                // JS not available, but continue anyway - we have cache
            }
            
            var principal = JwtTokenParser.ParseToken(accessToken);
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Log.Warning("MarkUserAsAuthenticated: Invalid token");
                principal = _anonymous;
                _tokenCache.TryRemove(circuitId, out _);
            }
            else
            {
                Log.Information("MarkUserAsAuthenticated: User authenticated {Name}", principal.Identity.Name);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

    public async Task MarkUserAsLoggedOut()
        {
            var circuitId = GetCircuitId();
            
            // Clear cache
            _tokenCache.TryRemove(circuitId, out _);
            
            // Clear localStorage
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "accessToken");
            }
            catch
            {
                // JS not available, but continue anyway
            }
            
            Log.Information("MarkUserAsLoggedOut: User logged out");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
