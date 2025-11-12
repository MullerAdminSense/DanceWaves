using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace DanceWaves.Infrastructure.Security
{
    public static class JwtTokenParser
    {
        public static ClaimsPrincipal? ParseToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                // Check if token is expired
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    return null;
                }
                
                // Create identity with authentication type "jwt" 
                // When authenticationType is provided and not empty, IsAuthenticated will be true
                var identity = new ClaimsIdentity(
                    jwtToken.Claims, 
                    "jwt", 
                    System.Security.Claims.ClaimTypes.Name, 
                    System.Security.Claims.ClaimTypes.Role);
                
                // If for some reason it's not authenticated, try with a different auth type
                if (!identity.IsAuthenticated)
                {
                    identity = new ClaimsIdentity(
                        jwtToken.Claims, 
                        "Bearer", 
                        System.Security.Claims.ClaimTypes.Name, 
                        System.Security.Claims.ClaimTypes.Role);
                }
                
                return new ClaimsPrincipal(identity);
            }
            catch
            {
                return null;
            }
        }

        public static string? GetUserId(string token)
        {
            var principal = ParseToken(token);
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
