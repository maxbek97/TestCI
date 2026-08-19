using TestCI.Aplication.Auth.Register;
using TestCI.Domain.Authentification;
using TestCI.Infrastructure.Authentification;

namespace TestCI.Aplication.Auth.Refresh
{
    public class RefreshHandler
    {
        private readonly IUserRepository _users;
        private readonly IRefreshTokenRepository _refreshTokens;
        private readonly IJwtService _jwtService;
        public RefreshHandler(IUserRepository users, IRefreshTokenRepository refreshTokens, IJwtService jwtService)
        {
            _users = users;
            _refreshTokens = refreshTokens;
            _jwtService = jwtService;
        }

        public async Task<RefreshResult> Handle(
            RefreshRequest request)
        {
            var refreshToken =
                await _refreshTokens.GetByToken(
                    request.RefreshToken);

            if (refreshToken == null)
                return RefreshResult.Failure(
                    "Invalid refresh token");

            if (!refreshToken.IsValid())
                return RefreshResult.Failure(
                    "Refresh token is expired or revoked");

            var user = await _users.GetById(refreshToken.UserId);

            if (user == null)
                return RefreshResult.Failure(
                    "User not found");

            var accessToken =
                _jwtService.GenerateToken(user);

            return RefreshResult.Success(
                accessToken);
        }
    }
}
