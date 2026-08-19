namespace TestCI.Aplication.Auth.Login
{
    public class LoginResult
    {
        public bool success { get; private set; }

        public string? AccessToken { get; private set; }

        public string? RefreshToken { get; private set; }

        public string? Message { get; private set; }

        private LoginResult()
        {
        }

        public static LoginResult Success(
            string accessToken,
            string refreshToken)
        {
            return new LoginResult
            {
                success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public static LoginResult Failure(string message)
        {
            return new LoginResult
            {
                success = false,
                Message = message
            };
        }
    }
}
