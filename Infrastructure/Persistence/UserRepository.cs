using TestCI.Aplication.Auth;
using Microsoft.EntityFrameworkCore;
using TestCI.Domain.Users;

namespace TestCI.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DigiRubContext _db;

        public UserRepository(DigiRubContext db)
        {
            _db = db;
        }

        public async Task<bool> ExistsByLogin(string login)
        {
            return await _db.Users
                .AnyAsync(x => x.Login == login);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _db.Users
                .AnyAsync(x => x.Email == email);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task Add(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
        
            public async Task<User?> GetById(int id)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
