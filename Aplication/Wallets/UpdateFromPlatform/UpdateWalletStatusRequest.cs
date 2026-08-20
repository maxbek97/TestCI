using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Wallets.UpdateFromPlatform
{
    public class UpdateWalletStatusRequest
    {
        public Guid ClientId { get; set; }
        public Guid Id_Dr { get; set; }
        public StatusWallet newStatus { get; set; }
    }
}
