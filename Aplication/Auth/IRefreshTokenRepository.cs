using TestCI.Domain.Authentification;

namespace TestCI.Aplication.Auth;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByToken(string token);

    Task Add(RefreshToken refreshToken);
    Task Save();
}
