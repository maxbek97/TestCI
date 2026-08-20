using TestCI.Aplication.Auth.Register;
using TestCI.Aplication.Clients.GetClients;
using TestCI.Domain.Clients;
using TestCI.Domain.Users;

namespace TestCI.Aplication.Clients.CreateClient
{
    public class CreateClientHandler
    {
        private readonly IClientRepository _clients;

        public CreateClientHandler(IClientRepository clients)
        {
            _clients = clients;
        }

        public async Task<CreateClientResult> Handle(
    CreateClientRequest request)
        {
            var new_mid = Guid.NewGuid();
            while (true)
            {
                if (!await _clients.ExistsByMid(new_mid)) break;
                else new_mid = Guid.NewGuid();
            }

            var client = new Client(
                new_mid, request.LastName, request.FirstName, request.MiddleName
                );

            try
            {
                await _clients.Create(client);
                await _clients.Save();
            }
            catch (Exception ex) {
                return CreateClientResult.Failure("Something went wrong");
            }

            return CreateClientResult.Success("Client created successfully");
        }
    }
}
