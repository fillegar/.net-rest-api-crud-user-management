using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using System.Threading.Tasks;
using User.API.Handler;
using User.Core;
using User.Core.Model;
using User.Service;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserInfoService _userInfoService;
        private readonly TokenCreationHandler _tokenHandler;

        public LoginController(UserInfoService userInfoService, TokenCreationHandler tokenHandler)
        {
            _userInfoService = userInfoService;
            _tokenHandler = tokenHandler;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Token>> Login([FromBody] UserLogin userLogin, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var user = await _userInfoService.GetUserInfoByEmailAsync(userLogin.Email!, cancellationToken);
            if (user is null || !TimeConstantComparer.IsEqual(user.Password ?? string.Empty, userLogin.Password ?? string.Empty))
            {
                return Unauthorized();
            }

            var token = _tokenHandler.CreateAccessToken(user);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);

            await _userInfoService.UpdateUserInfoAsync(user, cancellationToken);

            return Ok(token);
        }
    }
}
