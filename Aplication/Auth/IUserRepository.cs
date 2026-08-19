using TestCI.Domain.Users;
namespace TestCI.Aplication.Auth
{
    public interface IUserRepository
    {
        Task<bool> ExistsByLogin(string login);
        Task<bool> ExistsByEmail(string email);
        Task<User?> GetByEmail(string email);
        Task Add(User user);
    }
}
