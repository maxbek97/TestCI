using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Clients.GetWallets
{
    public class GetWalletClientResult
    {
        public bool success { get; set; } 
        public List<WalletResult> Clients { get; set; } = new();
        public string? message { get; set; }

        public static GetWalletClientResult Success(List<DrWallet> client_wallets)
        {
            var result = new GetWalletClientResult
            {
                success = true,
                message = $"Found {client_wallets.Count} wallets for this client"
            };

            foreach (var i in client_wallets)
            {
                var wallet = new WalletResult
                {
                    Id_DRw = i.Id_DRw,
                    BillId = i.BillId,
                    Status = i.Status
                };
                result.Clients.Add(wallet);
            }
            return result;
        }
        public static GetWalletClientResult Failure(string message)
        {
            return new GetWalletClientResult
            {
                success = false,
                Clients = new List<WalletResult>(),
                message = message
            };
        }
    }
    public class WalletResult
    {
        public Guid Id_DRw { get; set; }
        public Guid? BillId { get; set; }
        public StatusWallet Status { get; set; }
    }

    }
