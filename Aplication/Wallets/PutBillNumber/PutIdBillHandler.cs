using Microsoft.EntityFrameworkCore;
using TestCI.Aplication.Clients;
using TestCI.Aplication.Wallets.PutBillNumber;
using TestCI.Aplication.Wallets.UpdateFromPlatform;

namespace TestCI.Aplication.Wallets.PutBillNumber
{
    public class PutIdBillHandler
    {
        private readonly IClientRepository _clients;
        private readonly IWalletRepository _wallets;

        public PutIdBillHandler(
            IClientRepository clients,
            IWalletRepository wallets)
        {
            _clients = clients;
            _wallets = wallets;
        }

        public async Task<PutIdBillResult> Handle(
            PutIdBillRequest request)
        {
            var client = await _clients.GetByMid(request.ClientId);

            if (client == null)
            {
                return PutIdBillResult.Failure(
                    "Client not found.");
            }

            var wallet = await _wallets.GetByIdDr(request.Id_Dr);
            if (wallet == null)
            {
                return PutIdBillResult.Failure(
                    "Wallet with this platform ID doestn exists");
            }

            if (wallet.ClientId != request.ClientId)
            {
                return PutIdBillResult.Failure(
                    "Wallet does not belong to this client.");
            }

            try
            {
                wallet.SetBillId(request.Id_Bill);
                await _wallets.Save();
            }
            catch (InvalidOperationException ex)
            {
                return PutIdBillResult.Failure(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return PutIdBillResult.Failure(
                    "This bill ID is already assigned to another wallet.");
            }

            return PutIdBillResult.Success(
                "Bill`s id put successfully");
        }
    }
}
