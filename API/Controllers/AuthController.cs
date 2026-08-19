using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Auth.Register;
using TestCI.Aplication.Auth.Refresh;

namespace TestCI.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterHandler _registerHandler;
        private readonly RefreshHandler _refreshHandler;

        public AuthController(RegisterHandler registerHandler, RefreshHandler refreshHandler)
        {
            _registerHandler = registerHandler;
            _refreshHandler = refreshHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _registerHandler.Handle(request);

            if (!result.success)
                return BadRequest(new { message = result.Message});

            return Ok(new { message = result.Message });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await _refreshHandler.Handle(request);
            if (!result.success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new { accessToken = result.NewAccessToken });
        }


        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        //{
        //    var result = await _authService.LoginAsync(dto);

        //    if (!result.Success)
        //    {
        //        return Unauthorized(new
        //        {
        //            success = false,
        //            message = "Unauthorized"
        //        });
        //    }

        //    return Ok(new
        //    {
        //        success = true,
        //        accessToken = result.AccessToken,
        //        refreshToken = result.RefreshToken
        //    });

        //}

    }
}
