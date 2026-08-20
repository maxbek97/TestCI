using TestCI.Domain.Clients;
using TestCI.Domain.DrWallets;

namespace TestCI.Aplication
{
    public interface IWalletRepository
    {
        //Это все кошельки по клиенту
        Task<List<DrWallet>> Get(
            Guid mid);

        Task<DrWallet?> GetByIdDr(Guid Id_dr);
        Task Update(DrWallet wallet);
        Task<bool> ExistsById(Guid id_dr);

        Task Create(DrWallet wallet);

        Task Save();
    }
}
