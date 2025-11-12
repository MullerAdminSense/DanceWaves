using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace DanceWaves.Infrastructure.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
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
            catch (JSException)
            {
                // JS interop not available (server-side rendering) - use cache if available
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
            }
            catch (InvalidOperationException)
            {
                // JS interop not available (server-side rendering) - use cache if available
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
            }
            catch
            {
                // Any other error - use cache if available
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(_anonymous);
            }

            var principal = JwtTokenParser.ParseToken(token);
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                // Invalid token - clear cache
                _tokenCache.TryRemove(circuitId, out _);
                return new AuthenticationState(_anonymous);
            }

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
                principal = _anonymous;
                _tokenCache.TryRemove(circuitId, out _);
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
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
