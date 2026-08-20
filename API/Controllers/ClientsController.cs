using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ClientsController(GetClientsHandler handler)
        {
            _handler = handler;
        }
        public async Task<IActionResult> GetClients(
            [FromQuery] GetClientsRequest request)
        {
            var result = await _handler.Handle(request);

            return Ok(result);
        }
    }
}
