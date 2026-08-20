using Microsoft.EntityFrameworkCore;
using TestCI.Aplication.Clients;
using TestCI.Domain.Clients;

namespace TestCI.Infrastructure.Persistence;

public class ClientRepository : IClientRepository
{
    private readonly DigiRubContext _db;

    public ClientRepository(DigiRubContext db)
    {
        _db = db;
    }

    public async Task<List<Client>> Get(
        string? search,
        int page,
        int pageSize)
    {
        var query = _db.Clients
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.LastName.Contains(search) ||
                x.FisrtName.Contains(search) ||
                x.MiddleName.Contains(search));
        }

        return await query
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FisrtName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<Client?> GetByMid(Guid mid)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(x => x.Mid == mid);

        if (client == null)
            return null;

        var wallets = await _db.DrWallets
            .Where(x => x.ClientId == mid)
            .ToListAsync();

        client.LoadDrWallets(wallets);

        return client;
    }

    public async Task<int> Count(string? search)
    {
        var query = _db.Clients
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.LastName.Contains(search) ||
                x.FisrtName.Contains(search) ||
                x.MiddleName.Contains(search));
        }

        return await query.CountAsync();
    }

    public async Task Create(Client client)
    {
        _db.Clients.Add(client);
    }

    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsByMid(Guid mid)
    {
        return await _db.Clients
            .AnyAsync(x => x.Mid == mid);
    }
}