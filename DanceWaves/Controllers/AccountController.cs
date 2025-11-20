using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DanceWaves.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            // Remove authentication cookies
            await HttpContext.SignOutAsync("Cookies");

            // Remove user tokens here if using a token system
            // Exemplo: await _tokenService.RemoveUserTokens(User.Identity.Name);

            // Redireciona para a tela de login
            return Redirect("/login");
        }
    }
}
