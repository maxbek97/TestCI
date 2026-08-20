using TestCI.Aplication.Clients;
using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Wallets.UpdateFromPlatform
{
    public class UpdateStatusWalletHandler
    {
        private readonly IClientRepository _clients;
        private readonly IWalletRepository _wallets;

        public UpdateStatusWalletHandler(
            IClientRepository clients,
            IWalletRepository wallets)
        {
            _clients = clients;
            _wallets = wallets;
        }

        public async Task<UpdateWallerStatusResult> Handle(
            UpdateWalletStatusRequest request)
        {
            var client = await _clients.GetByMid(request.ClientId);

            if (client == null)
            {
                return UpdateWallerStatusResult.Failure(
                    "Client not found.");
            }

            var wallet = await _wallets.GetByIdDr(request.Id_Dr);
            if (wallet == null)
            {
                return UpdateWallerStatusResult.Failure(
                    "Wallet with this platform ID doestn exists");
            }

            if (wallet.ClientId != request.ClientId)
            {
                return UpdateWallerStatusResult.Failure(
                    "Wallet does not belong to this client.");
            }

            try
            {
                wallet.ChangeStatus(request.newStatus);
                await _wallets.Save();
            }
            catch (InvalidOperationException ex)
            {
                return UpdateWallerStatusResult.Failure($"Something went wrong + {ex.Message}");
            }


            return UpdateWallerStatusResult.Success(
                "Wallet status update successfully.");
        }
    }
}
