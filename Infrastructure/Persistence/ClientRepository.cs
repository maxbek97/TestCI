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
}