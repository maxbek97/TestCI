using Microsoft.EntityFrameworkCore;
using TestCI.Aplication;
using TestCI.Domain.DrWallets;

namespace TestCI.Infrastructure.Persistence
{
    public class WalletRepository: IWalletRepository
    {
        private readonly DigiRubContext _db;

        public WalletRepository(DigiRubContext db)
        {
            _db = db;
        }

        public async Task<List<DrWallet>> Get(Guid mid)
        {
            return await _db.DrWallets
                .Where(x => x.ClientId == mid)
                .ToListAsync();
        }
    }
}
