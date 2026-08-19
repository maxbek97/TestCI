using Microsoft.AspNetCore.Identity;
using TestCI.Infrastructure.Persistence;
using TestCI.Domain.Users;

namespace TestCI.Aplication.Auth.Register
{
    public class RegisterHandler
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterHandler(
            IUserRepository users,
            IPasswordHasher passwordHasher)
        {
            _users = users;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResult> Handle(
            RegisterRequest request)
        {
            if (await _users.ExistsByLogin(request.UserLogin))
                return RegisterResult.Failure(
                    "Login already exists");

            if (await _users.ExistsByEmail(request.UserEmail))
                return RegisterResult.Failure(
                    "Email already exists");

            var user = new User(request.UserLogin, request.UserEmail);

            user.SetPasswordHash(
                _passwordHasher.Hash(user, request.Password));

            await _users.Add(user);

            return RegisterResult.Success(
                "Registration successful");
        }
    }
}
