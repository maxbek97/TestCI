using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Auth.Register;

namespace TestCI.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterHandler _registerHandler;

        public AuthController(RegisterHandler registerHandler)
        {
            _registerHandler = registerHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _registerHandler.Handle(request);

            if (!result.success)
                return BadRequest(new { message = result.Message});

            return Ok(new { message = result.Message });
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
        //[HttpPost("refresh")]
        //public async Task<IActionResult> Refresh([FromBody] RefreshDTO dto)
        //{

        //    var result = await _authService.RefreshAsync(dto);

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
        //        accessToken = result.NewAccessToken,
        //    });

        //}
    }
}
