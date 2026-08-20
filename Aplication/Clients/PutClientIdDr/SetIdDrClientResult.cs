namespace TestCI.Aplication.Clients.PutClientIdDr
{
    public class SetIdDrClientResult
    {
        public bool success { get; private set; }

        public string? message { get; private set; }

        public static SetIdDrClientResult Success(string message)
        {
            return new SetIdDrClientResult
            {
                success = true,
                message = message
            };
        }

        public static SetIdDrClientResult Failure(string message)
        {
            return new SetIdDrClientResult
            {
                success = false,
                message = message
            };
        }
    }
}
