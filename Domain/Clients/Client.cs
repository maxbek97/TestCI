using TestCI.Domain.DrWallets;

namespace TestCI.Domain.Clients

{
    public class Client
    {
        private readonly List<DrWallet> _drWallets = new();

        public Guid Mid { get; private set; }

        public string LastName { get; private set; }

        public string FisrtName { get; private set; }

        public string MiddleName { get; private set; }

        public Guid? IdDr { get; private set; }

        public IReadOnlyCollection<DrWallet> DrWallets =>
            _drWallets.AsReadOnly();

        public Client(
            Guid mid,
            string lastName,
            string fisrtName,
            string middleName)
        {
            Mid = mid;
            LastName = lastName;
            FisrtName = fisrtName;
            MiddleName = middleName;
        }

        public void AddDrWallet(DrWallet wallet)
        {
            if (wallet.ClientId != Mid)
            {
                throw new InvalidOperationException(
                    "Ереыы тще нщгк цфддуе");
            }


            if (_drWallets.Any(x => x.Status != StatusWallet.Clsd))
            {
                throw new InvalidOperationException(
                    "active wallets cant be >1");
            }

            _drWallets.Add(wallet);
        }

        public void SetIdDr(Guid idDr)
        {
            IdDr = idDr;
        }
    }
}
