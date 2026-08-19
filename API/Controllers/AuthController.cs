using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Auth.Register;
using TestCI.Aplication.Auth.Refresh;
using TestCI.Aplication.Auth.Login;

namespace TestCI.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterHandler _registerHandler;
        private readonly RefreshHandler _refreshHandler;
        private readonly LoginHandler _loginHandler;

        public AuthController(RegisterHandler registerHandler, RefreshHandler refreshHandler, LoginHandler loginHandler)
        {
            _registerHandler = registerHandler;
            _refreshHandler = refreshHandler;
            _loginHandler = loginHandler;
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _loginHandler.Handle(request);

            if (!result.success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new
            {
                accessToken = result.AccessToken,
                refreshToken = result.RefreshToken
            });

        }

    }
}
