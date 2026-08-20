using TestCI.Aplication.Clients;
using TestCI.Domain.DrWallets;
using TestCI.Domain.Clients;
using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Wallets.CreateWallet
{

    public class CreateWalletHandler
    {
        private readonly IClientRepository _clients;
        private readonly IWalletRepository _wallets;

        public CreateWalletHandler(
            IClientRepository clients,
            IWalletRepository wallets)
        {
            _clients = clients;
            _wallets = wallets;
        }

        public async Task<CreateWalletResult> Handle(
            CreateWalletRequest request)
        {
            var client = await _clients.GetByMid(request.ClientId);

            if (client == null)
            {
                return CreateWalletResult.Failure(
                    "Client not found.");
            }

            if (await _wallets.ExistsById(request.Id_DRw))
            {
                return CreateWalletResult.Failure(
                    "Wallet with this platform ID already exists.");
            }

            DrWallet wallet;

            try
            {
                wallet = new DrWallet(
                    request.Id_DRw,
                    request.ClientId,
                    request.Status);

                client.AddDrWallet(wallet);

                await _wallets.Create(wallet);
                await _wallets.Save();
            }
            catch (InvalidOperationException ex)
            {
                return CreateWalletResult.Failure($"Something went wrong + {ex.Message}");
            }


            return CreateWalletResult.Success(
                "Wallet created successfully.");
        }
    }
}
