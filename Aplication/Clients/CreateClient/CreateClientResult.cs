using TestCI.Aplication.Auth.Login;

namespace TestCI.Aplication.Clients.CreateClient
{
    public class CreateClientResult
    {
        public bool success { get; private set; }
        public string? Message { get; private set; }

        public static CreateClientResult Success(string message)
        {
            return new CreateClientResult
            {
                success = true,
                Message = message
            };
        }

        public static CreateClientResult Failure(string message)
        {
            return new CreateClientResult
            {
                success = false,
                Message = message
            };
        }
    }
}
