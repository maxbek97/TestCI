using TestCI.Aplication.Auth;
using TestCI.Domain.Users;
using Microsoft.AspNetCore.Identity;
namespace TestCI.Infrastructure.Authentification
{
    public class PasswordService : IPasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new();

        public string Hash(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool Verify(
            User user,
            string passwordHash,
            string password)
        {
            var result = _hasher.VerifyHashedPassword(
                user,
                passwordHash,
                password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
