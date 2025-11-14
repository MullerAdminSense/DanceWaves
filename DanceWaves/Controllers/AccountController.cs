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
            // Remove cookies de autenticação
            await HttpContext.SignOutAsync("Cookies");

            // Exclua tokens do usuário aqui, se estiver usando um sistema de tokens
            // Exemplo: await _tokenService.RemoveUserTokens(User.Identity.Name);

            // Redireciona para a tela de login
            return Redirect("/login");
        }
    }
}
