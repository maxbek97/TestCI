using TestCI.Domain.Users;
namespace TestCI.Aplication.Auth

{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
