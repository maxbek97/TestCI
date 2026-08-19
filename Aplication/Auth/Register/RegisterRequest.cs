namespace TestCI.Aplication.Auth.Register
{
    public class RegisterRequest
    {
        public string UserLogin { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string UserEmail { get; set; } = null!;
    }
}
