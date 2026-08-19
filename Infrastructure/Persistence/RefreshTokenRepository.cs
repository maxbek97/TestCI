using Microsoft.EntityFrameworkCore;
using TestCI.Domain.Authentification;

namespace TestCI.Infrastructure.Persistence;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DigiRubContext _db;

    public RefreshTokenRepository(DigiRubContext db)
    {
        _db = db;
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await _db.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task Add(RefreshToken refreshToken)
    {
        _db.RefreshTokens.Add(refreshToken);
    }
    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}
