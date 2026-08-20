using TestCI.Aplication.Wallets.CreateWallet;

namespace TestCI.Aplication.Wallets.UpdateFromPlatform
{
    public class UpdateWallerStatusResult
    {
        public bool success { get; private set; }

        public string? message { get; private set; }

        public static UpdateWallerStatusResult Success(string message)
        {
            return new UpdateWallerStatusResult
            {
                success = true,
                message = message
            };
        }

        public static UpdateWallerStatusResult Failure(string message)
        {
            return new UpdateWallerStatusResult
            {
                success = false,
                message = message
            };
        }
    }
}
