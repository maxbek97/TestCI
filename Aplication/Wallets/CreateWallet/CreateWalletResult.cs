namespace TestCI.Aplication.Wallets.CreateWallet
{
    public class CreateWalletResult
    {
        public bool success { get; private set; }

        public string? message { get; private set; }

        public static CreateWalletResult Success(string message)
        {
            return new CreateWalletResult
            {
                success = true,
                message = message
            };
        }

        public static CreateWalletResult Failure(string message)
        {
            return new CreateWalletResult
            {
                success = false,
                message = message
            };
        }
    }
}
