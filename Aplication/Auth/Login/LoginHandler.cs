namespace TestCI.Aplication.Auth.Login
{
    public class LoginHandler
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokens;

        public LoginHandler(
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokens)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokens = refreshTokens;
        }

        public async Task<LoginResult> Handle(
            LoginRequest request)
        {
            var user = await _users.GetByEmail(
                request.UserEmail);

            if (user == null)
            {
                return LoginResult.Failure(
                    "Invalid email or password");
            }

            var passwordValid = _passwordHasher.Verify(
                user,
                user.PasswordHash,
                request.Password);

            if (!passwordValid)
            {
                return LoginResult.Failure(
                    "Invalid email or password");
            }

            var accessToken =
                _jwtService.GenerateToken(user);

            var refreshTokenString =
                _jwtService.GenerateRefreshToken();

            var refreshToken = new Domain.Authentification.RefreshToken(
                refreshTokenString,
                user.Id,
                DateTime.UtcNow.AddDays(7)
            );

            await _refreshTokens.Add(refreshToken);

            await _refreshTokens.Save();

            return LoginResult.Success(
                accessToken,
                refreshTokenString);
        }
    }
}
