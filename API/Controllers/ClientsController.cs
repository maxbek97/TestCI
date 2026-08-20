using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestCI.Aplication.Clients.CreateClient;
using TestCI.Aplication.Clients.GetClients;
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

        public ClientsController(GetClientsHandler handler, CreateClientHandler createClientHandler)
        {
            _handler = handler;
            _createClientHandler = createClientHandler;
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
    }
}
