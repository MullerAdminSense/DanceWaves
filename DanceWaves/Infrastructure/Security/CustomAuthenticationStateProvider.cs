using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace DanceWaves.Infrastructure.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private string? _cachedToken;
        private bool _tokenLoaded = false;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_tokenLoaded)
            {
                try
                {
                    _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");
                }
                catch
                {
                    // JS interop not available during prerendering
                    _cachedToken = null;
                }
                _tokenLoaded = true;
            }

            if (string.IsNullOrWhiteSpace(_cachedToken))
                return new AuthenticationState(_anonymous);

            var principal = JwtTokenParser.ParseToken(_cachedToken);
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                return new AuthenticationState(_anonymous);

            return new AuthenticationState(principal);
        }

        public async Task MarkUserAsAuthenticated(string accessToken)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", accessToken);
            _cachedToken = accessToken;
            _tokenLoaded = true;
            var principal = JwtTokenParser.ParseToken(accessToken) ?? _anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "accessToken");
            _cachedToken = null;
            _tokenLoaded = true;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
