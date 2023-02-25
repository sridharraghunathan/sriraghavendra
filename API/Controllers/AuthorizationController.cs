using System.Threading.Tasks;
using API.Entities;
using API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthorizationController : BaseApiController
    {
        // Intial Request from the client side for the Authentication verification of the user 
        // whether user is Admin or not
        [HttpPost]
        public async Task<ActionResult> AuthenticateToken(Token token)
        {
            UserDetails user = await TokenAuthorization.ValidateToken(token.IdToken);
            user.IsAdmin = AdminValidation.IsAdmin(user.Email);
            return Ok(user);
        }
    }
}