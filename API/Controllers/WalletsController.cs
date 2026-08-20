using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Wallets.CreateWallet;
using TestCI.Aplication.Wallets.PutBillNumber;
using TestCI.Aplication.Wallets.UpdateFromPlatform;

namespace TestCI.API.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly CreateWalletHandler _createWalletHandler;
        private readonly UpdateStatusWalletHandler _updateStatusWalletHandler;
        private readonly PutIdBillHandler _putIdBillHandler;

        public WalletController(
            CreateWalletHandler createWalletHandler, UpdateStatusWalletHandler updateStatusWalletHandler, PutIdBillHandler putIdBillHandler)
        {
            _createWalletHandler = createWalletHandler;
            _updateStatusWalletHandler = updateStatusWalletHandler;
            _putIdBillHandler = putIdBillHandler;
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

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateWallet(
            [FromBody] UpdateWalletStatusRequest request)
        {
            var result = await _updateStatusWalletHandler.Handle(request);

            if (!result.success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("setbill")]
        public async Task<IActionResult> SetWalletBill(
        [FromBody] PutIdBillRequest request)
        {
            var result = await _putIdBillHandler.Handle(request);

            if (!result.success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}