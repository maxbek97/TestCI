namespace TestCI.Domain.Users
{
    public class User
    {
        public int Id { get; private set; }

        public string Login { get; private set; }

        public string Email { get; private set; }

        public string PasswordHash { get; private set; }


        public User(
            string login,
            string email
            )
        {
            Login = login;
            Email = email;
        }

        public void SetPasswordHash(string hash)
        {
            PasswordHash = hash;
        }
    }
}
