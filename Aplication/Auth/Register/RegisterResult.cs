namespace TestCI.Aplication.Auth.Register
{
    public class RegisterResult
    {
        public bool success { get; init; }
        public string Message { get; init; } = null!;

        public static RegisterResult Success(string message)
        {
            return new RegisterResult
            {
                success = true,
                Message = message
            };
        }

        public static RegisterResult Failure(string message)
        {
            return new RegisterResult
            {
                success = false,
                Message = message
            };
        }
    }
}
