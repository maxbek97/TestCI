using TestCI.Aplication.Wallets.UpdateFromPlatform;

namespace TestCI.Aplication.Wallets.PutBillNumber
{
    public class PutIdBillResult
    {
        public bool success { get; private set; }

        public string? message { get; private set; }

        public static PutIdBillResult Success(string message)
        {
            return new PutIdBillResult
            {
                success = true,
                message = message
            };
        }

        public static PutIdBillResult Failure(string message)
        {
            return new PutIdBillResult
            {
                success = false,
                message = message
            };
        }
    }
}
