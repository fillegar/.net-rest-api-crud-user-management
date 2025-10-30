using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using User.Core.Model;
using User.Service;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserInfoService _userInfoService;

        public UserController(UserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfos(CancellationToken cancellationToken)
        {
            var users = await _userInfoService.GetUserInfosAsync(cancellationToken);
            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserInfo>> AddUserInfo([FromBody] UserInfo userInfo, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            await _userInfoService.AddUserInfoAsync(userInfo, cancellationToken);

            return CreatedAtAction(nameof(GetUserInfos), new { userInfo.Id }, userInfo);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfo userInfo, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            await _userInfoService.UpdateUserInfoAsync(userInfo, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUserInfo([FromBody] UserInfo userInfo, CancellationToken cancellationToken)
        {
            await _userInfoService.RemoveUserInfoAsync(userInfo, cancellationToken);
            return NoContent();
        }
    }
}
