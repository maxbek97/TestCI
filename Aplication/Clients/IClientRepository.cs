using TestCI.Domain.Clients;
namespace TestCI.Aplication.Clients

{
    public interface IClientRepository
    {
        Task<List<Client>> Get(
            string? search,
            int page,
            int pageSize);

        Task<int> Count(
            string? search);
    }
}
