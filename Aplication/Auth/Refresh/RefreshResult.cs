namespace TestCI.Aplication.Auth.Refresh
{

    public class RefreshResult
    {
        public bool success { get; private set; }

        public string? NewAccessToken { get; private set; }

        public string? Message { get; private set; }


        private RefreshResult()
        {
        }


        public static RefreshResult Success(
            string accessToken)
        {
            return new RefreshResult
            {
                success = true,
                NewAccessToken = accessToken
            };
        }


        public static RefreshResult Failure(
            string message)
        {
            return new RefreshResult
            {
                success = false,
                Message = message
            };
        }
    }
}
