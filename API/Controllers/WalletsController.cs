using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Wallets.CreateWallet;

namespace TestCI.API.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly CreateWalletHandler _createWalletHandler;

        public WalletController(
            CreateWalletHandler createWalletHandler)
        {
            _createWalletHandler = createWalletHandler;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWallet(
           [FromBody] CreateWalletRequest request)
        {
            var result = await _createWalletHandler.Handle(request);

            if (!result.success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}