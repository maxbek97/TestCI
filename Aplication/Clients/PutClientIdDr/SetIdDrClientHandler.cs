using Microsoft.EntityFrameworkCore;
using TestCI.Aplication.Wallets.PutBillNumber;

namespace TestCI.Aplication.Clients.PutClientIdDr
{
    public class SetIdDrClientHandler
    {
        private readonly IClientRepository _clients;
        public SetIdDrClientHandler(IClientRepository clients)
        {
            _clients = clients;
        }
        public async Task<SetIdDrClientResult> Handle(
            SetIdDrClientRequest request)
        {
            var client = await _clients.GetByMid(request.mid);

            if (client == null)
            {
                return SetIdDrClientResult.Failure(
                    "Client not found.");
            }

            try
            {
                client.SetIdDr(request.Id_Dr);
                await _clients.Save();
            }
            catch (InvalidOperationException ex)
            {
                return SetIdDrClientResult.Failure(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return SetIdDrClientResult.Failure(
                    "This ID Dr is already assigned to another wallet.");
            }

            return SetIdDrClientResult.Success(
                "DigitRub id put successfully");
        }
    }
}
