namespace TestCI.Infrastructure.Authentification
{
    public class AuthSettings
    {
        public TimeSpan Expires { get; set; }

        public string SecretKey { get; set; } = null!;
    }
}
