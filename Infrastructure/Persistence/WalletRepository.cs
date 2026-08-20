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
        public async Task<bool> ExistsById(Guid id_dr)
        {
            return await _db.DrWallets
                .AnyAsync(x => x.Id_DRw == id_dr);
        }

        public async Task Create(DrWallet wallet)
        {
            await _db.DrWallets.AddAsync(wallet);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
