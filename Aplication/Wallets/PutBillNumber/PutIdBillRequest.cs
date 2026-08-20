using TestCI.Domain.DrWallets;

namespace TestCI.Aplication.Wallets.PutBillNumber
{
    public class PutIdBillRequest
    {
        public Guid ClientId { get; set; }
        public Guid Id_Dr { get; set; }
        public Guid Id_Bill { get; set; }
    }
}
