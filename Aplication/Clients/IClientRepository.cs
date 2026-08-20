using TestCI.Domain.Clients;
namespace TestCI.Aplication.Clients

{
    public interface IClientRepository
    {
        Task<bool> ExistsByMid(Guid mid);
        Task<List<Client>> Get(
            string? search,
            int page,
            int pageSize);

        Task<int> Count(
            string? search);

        Task Create(Client client);

        Task Save();
    }
}
