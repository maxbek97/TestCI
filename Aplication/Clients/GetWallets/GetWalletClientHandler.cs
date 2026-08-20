using TestCI.Aplication.Clients.CreateClient;
using TestCI.Domain.Authentification;
using TestCI.Domain.Clients;
using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Clients.GetWallets
{
    public class GetWalletClientHandler
    {
        private readonly IWalletRepository _wallets;
        private readonly IClientRepository _clients;

        public GetWalletClientHandler(
            IWalletRepository wallets,
            IClientRepository clients)
        {
            _wallets = wallets;
            _clients = clients;
        }

        public async Task<GetWalletClientResult> Handle(
            GetWalletClientRequest request)
        {
            try
            {
                var clientExists = await _clients.ExistsByMid(request.midClient);

                if (!clientExists)
                {
                    return GetWalletClientResult.Failure(
                        "Client with specified MID was not found.");
                }

                var wallets = await _wallets.Get(request.midClient);

                return GetWalletClientResult.Success(wallets);
            }
            catch (Exception)
            {
                return GetWalletClientResult.Failure(
                    "Failed to get client wallets.");
            }
        }
    }
}
