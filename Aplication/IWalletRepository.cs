using TestCI.Domain.Clients;
using TestCI.Domain.DrWallets;

namespace TestCI.Aplication
{
    public interface IWalletRepository
    {
        Task<List<DrWallet>> Get(
            Guid mid);

    }
}
