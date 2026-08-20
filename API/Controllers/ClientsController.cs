using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Clients.CreateClient;
using TestCI.Aplication.Clients.GetClients;
using TestCI.Aplication.Clients.GetWallets;
using TestCI.Aplication.Clients.PutClientIdDr;
using TestCI.Aplication.Wallets.PutBillNumber;
using TestCI.Application.Clients.GetClients;

namespace TestCI.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly GetClientsHandler _handler;
        private readonly CreateClientHandler _createClientHandler;
        private readonly GetWalletClientHandler _getWalletClientHandler;
        private readonly SetIdDrClientHandler _setIdDrClientHandler;

        public ClientsController(GetClientsHandler handler, CreateClientHandler createClientHandler, GetWalletClientHandler getWalletClientHandler, SetIdDrClientHandler setIdDrClientHandler)
        {
            _handler = handler;
            _createClientHandler = createClientHandler;
            _getWalletClientHandler = getWalletClientHandler;
            _setIdDrClientHandler = setIdDrClientHandler;
        }
        public async Task<IActionResult> GetClients(
            [FromQuery] GetClientsRequest request)
        {
            var result = await _handler.Handle(request);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request) {
            var result = await _createClientHandler.Handle(request);
            if (!result.success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpGet("{mid}/wallets")]
        public async Task<IActionResult> GetWallets(Guid mid)
        {
            var request = new GetWalletClientRequest
            {
                midClient = mid
            };

            var result = await _getWalletClientHandler.Handle(request);

            if (!result.success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("setIdDr")]
        public async Task<IActionResult> SetWalletBill([FromBody] SetIdDrClientRequest request)
        {
            var result = await _setIdDrClientHandler.Handle(request);

            if (!result.success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
