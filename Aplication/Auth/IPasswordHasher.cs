using TestCI.Domain.Users;
namespace TestCI.Aplication.Auth
 
{
    public interface IPasswordHasher
    {
    string Hash(User user, string password);

    bool Verify(
            User user,
            string passwordHash,
            string password);
    }
}
