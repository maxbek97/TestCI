namespace TestCI.Domain.Authentification;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByToken(string token);

    Task Add(RefreshToken refreshToken);

    Task Save();
}
