using System.Text.Json.Serialization;
using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Wallets.CreateWallet
{
    public class CreateWalletRequest
    {
        public Guid ClientId { get; set; }
        public Guid Id_DRw { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusWallet Status { get; set; }
    }
}
